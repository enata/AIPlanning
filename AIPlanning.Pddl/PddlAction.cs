using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Pddl
{
    public sealed class PddlAction
    {
        private readonly string _name;
        private readonly PddlVariable[] _parameters;
        private readonly PddlLogicOperation _precondition;
        private readonly PddlLogicOperation _effect;

        public PddlAction(string name, IEnumerable<PddlVariable> parameters, PddlLogicOperation precondition, PddlLogicOperation effect)
        {
            if(string.IsNullOrEmpty(name)) throw new ArgumentException();
            if (parameters == null) throw new ArgumentNullException("parameters");
            if (precondition == null) throw new ArgumentNullException("precondition");
            if (effect == null) throw new ArgumentNullException("effect");

            _name = name;
            _precondition = precondition;
            _effect = effect;
            _parameters = parameters.ToArray();
        }

        public string Name
        {
            get { return _name; }
        }

        public PddlVariable[] Parameters
        {
            get { return _parameters; }
        }

        public PddlLogicOperation Precondition
        {
            get { return _precondition; }
        }

        public PddlLogicOperation Effect
        {
            get { return _effect; }
        }
    }
}