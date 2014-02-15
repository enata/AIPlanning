using System;
using System.Collections.Generic;
using AIPlanning.Core.Interfaces;

namespace AIPlanning.Search.Problems.Puzzle8
{
    public sealed class Puzzle8NoHeurisitcs : ICostFunction<Puzzle8State>
    {
        public double CalculateCost(ISearchNode<Puzzle8State> state, IProblem<Puzzle8State> problem)
        {
            return 0;
        }
    }

    public sealed class Puzzle8ManhattanHeuristic : ICostFunction<Puzzle8State>
    {
        private readonly Dictionary<int, Tuple<int, int>> _elementPositions = new Dictionary<int, Tuple<int, int>>(9);

        public Puzzle8ManhattanHeuristic(Puzzle8State goalState)
        {
            if (goalState == null) throw new ArgumentNullException("goalState");

            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    if(_elementPositions.ContainsKey(goalState.StateArray[i,j]))
                        throw new ArgumentException("Invalid goal state");
                    _elementPositions.Add(goalState.StateArray[i, j], new Tuple<int, int>(i, j));
                }
        }
   
        public double CalculateCost(ISearchNode<Puzzle8State> state, IProblem<Puzzle8State> problem)
        {
            if(state == null) throw new ArgumentNullException("state");

            double result = 0;
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                {
                    int currentElement = state.State.StateArray[i, j];
                    if(!_elementPositions.ContainsKey(currentElement))
                        throw new ArgumentException("Invalid state");
                    var goalPosition = _elementPositions[currentElement];
                    result += Math.Abs(goalPosition.Item1 - i) + Math.Abs(goalPosition.Item2 - j);
                }

            return result;
        }
    }
}