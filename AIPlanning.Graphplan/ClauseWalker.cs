using System;
using System.Collections.Generic;
using System.Linq;
using AIPlanning.Pddl;

namespace AIPlanning.Graphplan
{
    internal class ClauseWalker : IPddlLogicOperationVisitor
    {
        private readonly HashSet<PlanningGraph.Proposition> _included = new HashSet<PlanningGraph.Proposition>();
        private readonly HashSet<PlanningGraph.Proposition> _excluded = new HashSet<PlanningGraph.Proposition>();
        

        private bool _not;

        public void Walk(PddlLogicOperation operation)
        {
            operation.Accept(this);
        }

        public void Visit(PddlLogicOperationAnd operation)
        {
            foreach (var logicOperation in operation.Operation)
            {
                logicOperation.Accept(this);
            }
        }

        public HashSet<PlanningGraph.Proposition> Included { get { return _included; } }
        public HashSet<PlanningGraph.Proposition> Excluded { get { return _excluded; } } 

        public void Visit(PddlLogicOperationNot operation)
        {
            _not = true;
            operation.Operation.Accept(this);
        }

        public void Visit(PddlLogicOperationOr operation)
        {
            throw new NotImplementedException();
        }

        public void Visit(PddlPredicateCall operation)
        {
            var proposition = new PlanningGraph.Proposition(operation.Name,
                                                            operation.Parameters.Select(SubstituteParameter)
                                                                     .ToArray());
            if (_not)
            {
                _excluded.Add(proposition);
                _not = false;
            }
            else
            {
                _included.Add(proposition);
            }
        }

        protected virtual string SubstituteParameter(string parameter)
        {
            return parameter;
        }
    }
}