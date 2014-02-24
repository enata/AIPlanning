using AIPlanning.Pddl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Graphplan
{
    //TODO: mutex relationships
    //TODO: negative goals

    public sealed class ProblemSolutionSearcher
    {
        private readonly PddlProblem _problem;
        private readonly Dictionary<PlanningGraph.Action, HashSet<PlanningGraph.Proposition>> _actionPositiveEffects;
        private readonly Dictionary<PlanningGraph.Proposition, HashSet<PlanningGraph.Action>> _positiveEffectsActions;
        private readonly HashSet<PlanningGraph.Proposition> _goalPositive;
        private readonly HashSet<PlanningGraph.Proposition> _goalNegative; 
        private readonly List<HashSet<HashSet<PlanningGraph.Proposition>>> _nogood = new List<HashSet<HashSet<PlanningGraph.Proposition>>>(); 
        private readonly PropositionHashSetComparer _noGoodComparer = new PropositionHashSetComparer();

        private readonly List<PlanningGraph.Action> _plan;
        private readonly PlanningGraph _graph;


        public ProblemSolutionSearcher(PddlDomain domain, PddlProblem problem)
        {
            if (domain == null) throw new ArgumentNullException("domain");
            if (problem == null) throw new ArgumentNullException("problem");

            var substitutedActionsBuilder = new SubstitutedActionsBuilder(domain.Constants.Union(problem.Objects),
                                                                          domain.Actions);
            var effectsExtractor = new ActionEffectsExtractor(substitutedActionsBuilder.Substitutions);
            _actionPositiveEffects = effectsExtractor.ActionPositiveEffects;
            _positiveEffectsActions = effectsExtractor.PositiveEffectActions;
            _graph = new PlanningGraph(problem.PddlStatements, substitutedActionsBuilder.Substitutions, effectsExtractor);
            _noGoodComparer = new PropositionHashSetComparer();
            _nogood.Add(new HashSet<HashSet<PlanningGraph.Proposition>>(_noGoodComparer));

            _problem = problem;
            SetGoal(out _goalPositive, out _goalNegative);

            if(!TryExpandToGoal())
                return;

            _plan = BuildPlan();
        }

        public List<PlanningGraph.Action> Plan
        {
            get { return _plan; }
        }

        public PlanningGraph Graph
        {
            get { return _graph; }
        }

        private List<PlanningGraph.Action> BuildPlan()
        {
            var prevFailureSetSize = Graph.IsFixedPoint() ? _nogood[_nogood.Count - 1].Count : 0;
            var plan = Extract(_goalPositive, Graph.PropositionLayers.Count - 1);
            while (plan == null)
            {
                Graph.Expand();
                _nogood.Add(new HashSet<HashSet<PlanningGraph.Proposition>>(_noGoodComparer));
                plan = Extract(_goalPositive, Graph.PropositionLayers.Count - 1);
                if (plan == null && Graph.IsFixedPoint())
                {
                    var currentFailureSetSize = _nogood[_nogood.Count - 1].Count;
                    if (prevFailureSetSize == currentFailureSetSize)
                        return null;
                    prevFailureSetSize = currentFailureSetSize;
                }
            }
            return plan;
        }

        private bool TryExpandToGoal()
        {
            var lastLayer = Graph.PropositionLayers[Graph.PropositionLayers.Count - 1];

            while (!Graph.IsFixedPoint() && !CheckGoal(lastLayer, _goalPositive))
            {
                Graph.Expand();
                lastLayer = Graph.PropositionLayers[Graph.PropositionLayers.Count - 1];
                _nogood.Add(new HashSet<HashSet<PlanningGraph.Proposition>>(_noGoodComparer));
            }

            var result = CheckGoal(lastLayer, _goalPositive);
            return result;
        }

        private List<PlanningGraph.Action> Search(HashSet<PlanningGraph.Proposition> goals, List<PlanningGraph.Action> partialPlan, int level)
        {
            if (!goals.Any())
            {
                var plan = Extract(goals, level - 1);
                if (plan != null)
                    plan.AddRange(partialPlan);
                return plan;
            }
            var goal = goals.First();
            var resolvers = _positiveEffectsActions[goal].Intersect(Graph.ActionLayers[level - 1].Elements)
                                                         .ToArray();
            if (!resolvers.Any())
                return null;
            foreach (var resolver in resolvers)
            {
                var newPlan = new List<PlanningGraph.Action>(partialPlan) {resolver};
                var next = Search(
                    new HashSet<PlanningGraph.Proposition>(goals.Except(_actionPositiveEffects[resolver])), newPlan,
                    level);
                if (next != null)
                    return next;
            }
            return null;
        }

        private List<PlanningGraph.Action> Extract(HashSet<PlanningGraph.Proposition> subGoals, int level)
        {
            if(level == 0)
                return new List<PlanningGraph.Action>();

            if (_nogood[level].Contains(subGoals))
                return null;

            var result = Search(subGoals, new List<PlanningGraph.Action>(), level);
            if (result == null)
                _nogood[level].Add(subGoals);
            return result;
        }

        private bool CheckGoal(PlanningGraph.Layer<PlanningGraph.Proposition> layer, IEnumerable<PlanningGraph.Proposition> goalPositive)
        {
            var result = layer.Elements.IsSupersetOf(goalPositive);
            return result;
        }

        private void SetGoal(out HashSet<PlanningGraph.Proposition> goalPositive,
                             out HashSet<PlanningGraph.Proposition> goalNegative)
        {
            var walker = new ClauseWalker();
            walker.Walk(_problem.Goal);
            goalPositive = new HashSet<PlanningGraph.Proposition>(walker.Included);
            goalNegative = new HashSet<PlanningGraph.Proposition>(walker.Excluded);
        }
    }
}
