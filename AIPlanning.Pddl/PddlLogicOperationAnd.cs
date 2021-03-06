﻿using System.Collections.Generic;

namespace AIPlanning.Pddl
{
    public sealed class PddlLogicOperationAnd : PddlLogicOperationMultiOperand
    {
        public PddlLogicOperationAnd(IEnumerable<PddlLogicOperation> operation) : base(operation)
        {
        }

        public override void Accept(IPddlLogicOperationVisitor visitor)
        {
            visitor.Visit(this);
        }
    }
}