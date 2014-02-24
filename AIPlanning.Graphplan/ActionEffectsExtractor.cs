using System.Collections.Generic;
using AIPlanning.Pddl;

namespace AIPlanning.Graphplan
{
    internal sealed class ActionEffectsExtractor
    {
        private readonly Dictionary<PlanningGraph.Action, HashSet<PlanningGraph.Proposition>> _actionPositiveEffects;
        private readonly Dictionary<PlanningGraph.Action, HashSet<PlanningGraph.Proposition>> _actionNegativeEffects;
        private readonly Dictionary<PlanningGraph.Proposition, HashSet<PlanningGraph.Action>> _positiveEffectActions;

        public ActionEffectsExtractor(Dictionary<PlanningGraph.Action, PddlAction> substitutedActions)
        {
            _actionPositiveEffects = new Dictionary<PlanningGraph.Action, HashSet<PlanningGraph.Proposition>>(substitutedActions.Count);
            _actionNegativeEffects = new Dictionary<PlanningGraph.Action, HashSet<PlanningGraph.Proposition>>(substitutedActions.Count);
            _positiveEffectActions = new Dictionary<PlanningGraph.Proposition, HashSet<PlanningGraph.Action>>();

            foreach (var substitutedAction in substitutedActions)
            {
                var substitutionDictionary = new SubstitutionDictionary(substitutedAction.Value,
                                                                        substitutedAction.Key.Parameters);
                var clauseWalker = new SubstitutingClauseWalker(substitutionDictionary.DictionaryInstance);
                clauseWalker.Walk(substitutedAction.Value.Effect);
                ActionPositiveEffects[substitutedAction.Key] = clauseWalker.Included;
                ActionNegativeEffects[substitutedAction.Key] = clauseWalker.Excluded;

                foreach (var proposition in clauseWalker.Included)
                {
                    if (!PositiveEffectActions.ContainsKey(proposition))
                        PositiveEffectActions[proposition] = new HashSet<PlanningGraph.Action>();
                    PositiveEffectActions[proposition].Add(substitutedAction.Key);
                }
            }
        }

        public Dictionary<PlanningGraph.Action, HashSet<PlanningGraph.Proposition>> ActionPositiveEffects
        {
            get { return _actionPositiveEffects; }
        }

        public Dictionary<PlanningGraph.Action, HashSet<PlanningGraph.Proposition>> ActionNegativeEffects
        {
            get { return _actionNegativeEffects; }
        }

        public Dictionary<PlanningGraph.Proposition, HashSet<PlanningGraph.Action>> PositiveEffectActions
        {
            get { return _positiveEffectActions; }
        }
    }
}