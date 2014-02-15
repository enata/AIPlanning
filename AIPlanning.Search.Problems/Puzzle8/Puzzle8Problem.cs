using System;
using AIPlanning.Core.Interfaces;

namespace AIPlanning.Search.Problems.Puzzle8
{
    public sealed class Puzzle8Problem : IProblem<Puzzle8State>
    {
        private readonly Puzzle8State _goalState;
        private readonly ISearchNode<Puzzle8State> _initialState;

        public Puzzle8Problem(Puzzle8State goalState, Puzzle8State initialState)
        {
            _goalState = goalState;
            _initialState = new Puzzle8Node(initialState);
        }

        public ISearchNode<Puzzle8State> InitialState
        {
            get { return _initialState; }
        }

        public Puzzle8State GoalState
        {
            get { return _goalState; }
        }

        public bool GoalTest(ISearchNode<Puzzle8State> state)
        {
            if (state == null) throw new ArgumentNullException("state");
            if (state.State.StateArray.GetLength(0) != 3 || state.State.StateArray.GetLength(1) != 3)
                throw new ArgumentException("Invalid state array size");

            if (state.State.EmptyStatePosition.Equals(_goalState.EmptyStatePosition))
            {
                return CheckStateArray(state.State.StateArray);
            }
            return false;
        }

        private bool CheckStateArray(int[,] stateArray)
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 3; j++)
                    if (stateArray[i, j] != _goalState.StateArray[i, j])
                        return false;
            return true;
        }
    }
}