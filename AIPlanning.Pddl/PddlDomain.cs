using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Pddl
{
    public sealed class PddlDomain
    {
        private readonly string _name;
        private readonly string[] _types;
        private readonly Dictionary<string, PddlVariable> _nameVariableDictionary;
        private readonly Dictionary<string, PddlVariable[]> _typeVariableDictionary;
        private readonly Dictionary<string, PddlPredicate> _namePredicateDictionary;
        private readonly Dictionary<string, PddlAction> _nameActionDictionary; 


        public PddlDomain(string name, IEnumerable<string> types, IEnumerable<PddlVariable> constants, IEnumerable<PddlPredicate> predicates, IEnumerable<PddlAction> actions)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (types == null) throw new ArgumentNullException("types");
            if (constants == null) throw new ArgumentNullException("constants");
            if (predicates == null) throw new ArgumentNullException("predicates");
            if (actions == null) throw new ArgumentNullException("actions");

            _name = name;
            _types = types.ToArray();
            var enumeratedConstants = constants.ToArray();
            _nameVariableDictionary = enumeratedConstants.ToDictionary(c => c.Name);
            _typeVariableDictionary = enumeratedConstants.GroupBy(c => c.Type)
                                                         .ToDictionary(c => c.Key, c => c.ToArray());
            _namePredicateDictionary = predicates.ToDictionary(p => p.Name);
            _nameActionDictionary = actions.ToDictionary(a => a.Name);
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<string> Types
        {
            get { return _types; }
        }

        public IEnumerable<PddlVariable> Constants { get { return _nameVariableDictionary.Values; } }
        public IEnumerable<PddlPredicate> Predicates { get { return _namePredicateDictionary.Values; } }
        public IEnumerable<PddlAction> Actions { get { return _nameActionDictionary.Values; } } 

        public bool TryGetPredicateByName(string name, out PddlPredicate result)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException();

            bool contained = _namePredicateDictionary.TryGetValue(name, out result);
            return contained;
        }

        public bool TryGetConstantByName(string name, out PddlVariable result)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException();

            bool contained = _nameVariableDictionary.TryGetValue(name, out result);
            return contained;
        }

        public bool TryGetActionByName(string name, out PddlAction result)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException();

            bool contained = _nameActionDictionary.TryGetValue(name, out result);
            return contained;
        }

        public IEnumerable<PddlVariable> GetConstantsByType(string type)
        {
            if(string.IsNullOrEmpty(type)) throw new ArgumentException();

            return _typeVariableDictionary.ContainsKey(type)
                       ? _typeVariableDictionary[type]
                       : Enumerable.Empty<PddlVariable>();
        }
    }
}