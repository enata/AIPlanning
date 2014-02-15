using System.Collections.Generic;

namespace AIPlanning.Core.Interfaces
{
    public interface IPath<T>
    {
        IEnumerable<ISearchNode<T>> Steps { get; }
        double Cost { get; }
    }
}