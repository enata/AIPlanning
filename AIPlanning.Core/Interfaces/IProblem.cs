namespace AIPlanning.Core.Interfaces
{
    public interface IProblem<T>
    {
        ISearchNode<T> InitialState { get; }
        bool GoalTest(ISearchNode<T> state);
    }
}