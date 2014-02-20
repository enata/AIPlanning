using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Pddl
{
    public abstract class PddlLogicOperationMultiOperand : PddlLogicOperation
    {
        private readonly PddlLogicOperation[] _operation;

        protected PddlLogicOperationMultiOperand(IEnumerable<PddlLogicOperation> operation)
        {
            if (operation == null) throw new ArgumentNullException("operation");

            _operation = operation.ToArray();
        }

        public IEnumerable<PddlLogicOperation> Operation
        {
            get { return _operation; }
        }
    }
}