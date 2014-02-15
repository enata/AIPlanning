using System.Collections.Generic;

namespace AIPlanning.Core.Interfaces
{
    public interface ISearchNode<out T>
    {
        T State { get; }

        IEnumerable<ISearchNode<T>> Expand();
    }
}