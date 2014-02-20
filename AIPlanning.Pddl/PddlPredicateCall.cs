using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Pddl
{
    public sealed class  PddlPredicateCall : PddlLogicOperation
    {
        private readonly string _name;
        private readonly string[] _parameters;


        public PddlPredicateCall(string name, IEnumerable<string> parameters)
        {
            if (parameters == null) throw new ArgumentNullException("parameters");
            if(string.IsNullOrEmpty(name)) throw new ArgumentException();

            _name = name;
            _parameters = parameters.ToArray();
        }

        public string Name
        {
            get { return _name; }
        }

        public string[] Parameters
        {
            get { return _parameters; }
        }
    }
}