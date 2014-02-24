namespace AIPlanning.Pddl
{
    public abstract class PddlLogicOperation
    {
        public abstract void Accept(IPddlLogicOperationVisitor visitor);
    }
}