using System;
using System.Collections.Generic;
using AIPlanning.Core.Interfaces;

namespace AIPlanning.Search.Problems.Puzzle8
{
    public sealed class Puzzle8Node : ISearchNode<Puzzle8State>
    {
        private static readonly Tuple<int, int>[] EmptySpaceMovement = new[]
            {
                new Tuple<int, int>(-1, 0), new Tuple<int, int>(1, 0), new Tuple<int, int>(0, -1),
                new Tuple<int, int>(0, 1)
            };

        private readonly Puzzle8State _state;


        public Puzzle8Node(Puzzle8State state)
        {
            if (state == null) throw new ArgumentNullException("state");

            _state = state;
        }

        public Puzzle8State State
        {
            get { return _state; }
        }

        public IEnumerable<ISearchNode<Puzzle8State>> Expand()
        {
            foreach (var movement in EmptySpaceMovement)
            {
                int newX = _state.EmptyStatePosition.Item1 + movement.Item1;
                int newY = _state.EmptyStatePosition.Item2 + movement.Item2;
                if (newX < 0 || newX > 2 || newY < 0 || newY > 2)
                    continue;
                Puzzle8State newState = BuildNewState(new Tuple<int, int>(newX, newY));
                var node = new Puzzle8Node(newState);
                yield return node;
            }
        }

        private Puzzle8State BuildNewState(Tuple<int, int> emptySpaceNextPosition)
        {
            var newArray = new int[3,3];
            Buffer.BlockCopy(_state.StateArray, 0, newArray, 0, _state.StateArray.Length * sizeof(int));
            int currentX = _state.EmptyStatePosition.Item1;
            int currentY = _state.EmptyStatePosition.Item2;
            int tmp = newArray[currentX, currentY];
            int nextX = emptySpaceNextPosition.Item1;
            int nextY = emptySpaceNextPosition.Item2;
            newArray[currentX, currentY] = newArray[nextX, nextY];
            newArray[nextX, nextY] = tmp;
            var result = new Puzzle8State(newArray, emptySpaceNextPosition);
            return result;
        }

        private bool Equals(Puzzle8Node other)
        {
            return _state.Equals(other._state);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Puzzle8Node && Equals((Puzzle8Node) obj);
        }

        public override int GetHashCode()
        {
            return _state.GetHashCode();
        }
    }
}