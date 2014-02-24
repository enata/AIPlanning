using System.Collections.Generic;
using System.Linq;
using AIPlanning.Pddl;

namespace AIPlanning.Graphplan
{
    internal sealed class SubstitutionDictionary
    {
        private readonly Dictionary<string, string> _dictionary;

        public SubstitutionDictionary(PddlAction action, IEnumerable<string> substitutions)
        {
            _dictionary = new Dictionary<string, string>(action.Parameters.Length);
            var substitutionsArray = substitutions.ToArray();

            for (int i = 0; i < substitutionsArray.Length; i++)
            {
                _dictionary[action.Parameters[i].Name] = substitutionsArray[i];
            }
        }

        public Dictionary<string, string> DictionaryInstance { get { return _dictionary; } }
    }
}