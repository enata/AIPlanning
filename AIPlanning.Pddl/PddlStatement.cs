using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Pddl
{
    public sealed class PddlStatement
    {
        private readonly PddlPredicate _predicate;
        private readonly PddlVariable[] _parametersInitialization;

        public PddlStatement(PddlPredicate predicate, IEnumerable<PddlVariable> parametersInitialization)
        {
            if (predicate == null) throw new ArgumentNullException("predicate");
            if (parametersInitialization == null) throw new ArgumentNullException("parametersInitialization");

            _predicate = predicate;
            _parametersInitialization = parametersInitialization.ToArray();

            if (predicate.Parameters.Count() != _parametersInitialization.Length)
                throw new ArgumentException("Invalid parameters number");
        }

        public PddlPredicate Predicate
        {
            get { return _predicate; }
        }

        public IEnumerable<PddlVariable> ParametersInitialization
        {
            get { return _parametersInitialization; }
        }
    }
}
