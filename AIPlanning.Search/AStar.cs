using AIPlanning.Core;
using AIPlanning.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Search
{
    public sealed class AStar<T> : ISearch<T>
    {
        private readonly ICostFunction<T> _heuristic;

        public AStar(ICostFunction<T> heuristic)
        {
            if(heuristic == null)
                throw new ArgumentNullException("heuristic");

            _heuristic = heuristic;
        }

        public IPath<T> Find(IProblem<T> problem)
        {
            if (problem == null)
                throw new ArgumentNullException("problem");

            var fringe = new SortedList<double, ISearchNode<T>>(new CostComparer()) {{0.0, problem.InitialState}}; // to be replaced with minheap
            var pathTracker = new PathTracker<T>();
            pathTracker.TrackNode(problem.InitialState, null);

            while (fringe.Any())
            {
                ISearchNode<T> currentNode = fringe.First()
                                                .Value;
                fringe.RemoveAt(0);

                if (problem.GoalTest(currentNode))
                {
                    var result = pathTracker.GetPath(currentNode);
                    return result;
                }

                foreach (ISearchNode<T> successor in currentNode.Expand())
                {
                    bool isTracked = pathTracker.IsTracked(successor);
                    if (!isTracked || pathTracker.GetDepth(currentNode) + 1 < pathTracker.GetDepth(successor))
                    {
                        if (isTracked)
                        {
                            var nodeIndex = fringe.IndexOfValue(currentNode);
                            fringe.RemoveAt(nodeIndex);
                        }
                        pathTracker.TrackNode(successor, currentNode);
                        double cost = CostOf(successor, problem, pathTracker);
                        fringe.Add(cost, successor);  
                    }
                }
            }
            return Path<T>.Empty;
        }

        public int CountStatesAwayFrom(ISearchNode<T> initialState, int steps)
        {
            if (initialState == null) throw new ArgumentNullException("initialState");
            if(steps < 0) throw new ArgumentException("Invalid steps number");

            var fringe = new SortedList<double, ISearchNode<T>>(new CostComparer()) { { 0.0, initialState } };
            var pathTracker = new PathTracker<T>();
            pathTracker.TrackNode(initialState, null);

            while (fringe.Any())
            {
                ISearchNode<T> currentNode = fringe.First()
                                                .Value;
                fringe.RemoveAt(0);
                if (pathTracker.GetDepth(currentNode) == steps)
                    return fringe.Count + 1;

                foreach (ISearchNode<T> successor in currentNode.Expand())
                {
                    if (!pathTracker.IsTracked(successor) || pathTracker.GetDepth(currentNode) + 1 < pathTracker.GetDepth(successor))
                    {
                        pathTracker.TrackNode(successor, currentNode);
                        double cost = pathTracker.GetDepth(successor);
                        fringe.Add(cost, successor);
                    }
                }
            }
            return 0;
        }

        private double CostOf(ISearchNode<T> state, IProblem<T> problem, PathTracker<T> tracker)
        {
            int nodeDepth = tracker.GetDepth(state);
            double result = nodeDepth + _heuristic.CalculateCost(state, problem);
            return result;
        }

        private sealed class CostComparer : IComparer<double>
        {
            public int Compare(double x, double y)
            {
                return x < y ? -1 : 1;
            }
        }
    }
}