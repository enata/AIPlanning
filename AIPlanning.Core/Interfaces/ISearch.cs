namespace AIPlanning.Core.Interfaces
{
    public interface ISearch<T>
    {
        IPath<T> Find(IProblem<T> problem);
    }
}