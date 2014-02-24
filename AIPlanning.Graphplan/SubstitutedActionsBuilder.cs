using System.Collections.Generic;
using System.Linq;
using AIPlanning.Pddl;
using AIPlanning.Utils;

namespace AIPlanning.Graphplan
{
    internal sealed class SubstitutedActionsBuilder
    {
        private readonly Dictionary<string, string[]> _typeVariableDictionary;
        private readonly PddlAction[] _domainActions;
        private readonly Dictionary<PlanningGraph.Action, PddlAction> _substitutions;

        public SubstitutedActionsBuilder(IEnumerable<PddlVariable> objects, IEnumerable<PddlAction> domainActions)
        {
            _typeVariableDictionary = objects.GroupBy(v => v.Type)
                                             .ToDictionary(grouping => grouping.Key,
                                                           grouping => grouping.Select(v => v.Name)
                                                                               .ToArray());
            _domainActions = domainActions.ToArray();
            _substitutions = GetAllSubstitutedActions();
        }

        public Dictionary<PlanningGraph.Action, PddlAction> Substitutions
        {
            get { return _substitutions; }
        }

        private Dictionary<PlanningGraph.Action, PddlAction> GetAllSubstitutedActions( )
        {
            var result = _domainActions.SelectMany(da => FindSubstitutions(da, _typeVariableDictionary)
                                                             .Select(
                                                                 s =>
                                                                 new KeyValuePair<PlanningGraph.Action, PddlAction>(
                                                                     new PlanningGraph.Action(da.Name, s.ToArray()), da)))
                                       .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            return result;

        }

        private IEnumerable<IEnumerable<string>> FindSubstitutions(PddlAction action, Dictionary<string, string[]> typeVariableDictionary)
        {
            var parameters = action.Parameters;
            var variableSubstitutions = new Queue<IEnumerable<string>>(parameters.Select(p => FindSubstitutions(p, typeVariableDictionary)));
            var result = Substitute(variableSubstitutions);
            return result;
        }

        private IEnumerable<IEnumerable<string>> Substitute(Queue<IEnumerable<string>> substitutions)
        {
            if (!substitutions.Any())
                return Enumerable.Empty<IEnumerable<string>>();
            var currentSubstitutions = substitutions.Dequeue();
            var otherSubstitutions = Substitute(substitutions);
            var result = otherSubstitutions.Any()
                             ? currentSubstitutions.SelectMany(cs => otherSubstitutions.Select(os => cs.ToEnumerable()
                                                                                                       .Concat(os)))
                             : currentSubstitutions.Select(cs => cs.ToEnumerable());
            return result;
        }

        private IEnumerable<string> FindSubstitutions(PddlVariable parameter, Dictionary<string, string[]> typeVariableDictionary)
        {
            string[] substitutions;
            var result = typeVariableDictionary.TryGetValue(parameter.Type, out substitutions) ? substitutions : Enumerable.Empty<string>();
            return result;
        }
    }
}