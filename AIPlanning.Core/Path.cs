using System;
using System.Collections.Generic;
using System.Linq;
using AIPlanning.Core.Interfaces;

namespace AIPlanning.Core
{
    public sealed class Path<T> : IPath<T>
    {
        public static Path<T> Empty = new Path<T>(Enumerable.Empty<ISearchNode<T>>(), 0.0);

        private readonly double _cost;
        private readonly List<ISearchNode<T>> _steps;

        public Path(IEnumerable<ISearchNode<T>> path, double cost)
        {
            if (path == null)
                throw new ArgumentNullException("path");

            _cost = cost;
            _steps = new List<ISearchNode<T>>(path);
        }

        public IEnumerable<ISearchNode<T>> Steps
        {
            get { return _steps; }
        }

        public double Cost
        {
            get { return _cost; }
        }
    }
}