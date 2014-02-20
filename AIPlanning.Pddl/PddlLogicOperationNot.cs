using System;

namespace AIPlanning.Pddl
{
    public sealed class PddlLogicOperationNot : PddlLogicOperation
    {
        private readonly PddlLogicOperation _operation;

        public PddlLogicOperationNot(PddlLogicOperation operation)
        {
            if (operation == null) throw new ArgumentNullException("operation");

            _operation = operation;
        }

        public PddlLogicOperation Operation
        {
            get { return _operation; }
        }
    }
}