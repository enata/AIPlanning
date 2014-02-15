namespace AIPlanning.Core.Interfaces
{
    public interface ICostFunction<T>
    {
        double CalculateCost(ISearchNode<T> state, IProblem<T> problem);
    }
}