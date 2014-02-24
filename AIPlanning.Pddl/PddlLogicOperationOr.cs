using System.Collections.Generic;

namespace AIPlanning.Pddl
{
    public sealed class PddlLogicOperationOr : PddlLogicOperationMultiOperand
    {
        public PddlLogicOperationOr(IEnumerable<PddlLogicOperation> operation) : base(operation)
        {
        }

        public override void Accept(IPddlLogicOperationVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}