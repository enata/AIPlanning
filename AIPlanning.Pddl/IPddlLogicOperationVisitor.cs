namespace AIPlanning.Pddl
{
    public interface IPddlLogicOperationVisitor
    {
        void Visit(PddlLogicOperationAnd operation);
        void Visit(PddlLogicOperationNot operation);
        void Visit(PddlLogicOperationOr operation);
        void Visit(PddlPredicateCall operation);
    }
}