using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Pddl
{
    public sealed class PddlPredicate
    {
        private readonly string _name;
        private readonly PddlVariable[] _parameters; 

        public PddlPredicate(string name, IEnumerable<PddlVariable> parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException("name");

            _name = name;
            _parameters = parameters.ToArray();
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<PddlVariable> Parameters
        {
            get { return _parameters; }
        }
    }
}