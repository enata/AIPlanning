using System;

namespace AIPlanning.Search.Problems.Puzzle8
{
    public sealed class Puzzle8State
    {
        
        private readonly Tuple<int, int> _emptyStatePosition;
        private readonly int[,] _stateArray;
        private readonly int _arrayHash;

        public Puzzle8State(int[,] stateArray, Tuple<int, int> emptyStatePosition)
        {
            if (stateArray == null) throw new ArgumentNullException("stateArray");
            if (emptyStatePosition == null) throw new ArgumentNullException("emptyStatePosition");
            if (stateArray.GetLength(0) != 3 || stateArray.GetLength(1) != 3)
                throw new ArgumentException("Invalid array size");
            if (emptyStatePosition.Item1 < 0 || emptyStatePosition.Item1 > 2 || emptyStatePosition.Item2 < 0 ||
                emptyStatePosition.Item2 > 2)
                throw new ArgumentException("Invalid empty space position");

            _stateArray = stateArray;
            _emptyStatePosition = emptyStatePosition;
            _arrayHash = CalculateArrayHash();
        }

        public Puzzle8State(int[,] stateArray) : this(stateArray, GetEmptyPosition(stateArray))
        {
            
        }

        private static Tuple<int, int> GetEmptyPosition(int[,] stateArray)
        {
            if (stateArray == null) throw new ArgumentNullException("stateArray");

            for (int i = 0; i < stateArray.GetLength(0); i++)
                for (int j = 0; j < stateArray.GetLength(1); j++)
                {
                    if (stateArray[i, j] == 0)
                    {
                        var emptyPosition = new Tuple<int, int>(i, j);
                        return emptyPosition;
                    }
                }
            throw new ArgumentException("Invalid state");
        }

        public int[,] StateArray
        {
            get { return _stateArray; }
        }

        public Tuple<int, int> EmptyStatePosition
        {
            get { return _emptyStatePosition; }
        }

        private bool Equals(Puzzle8State other)
        {
            return _emptyStatePosition.Equals(other._emptyStatePosition) && CheckArrayEquality(other._stateArray);
        }

        private bool CheckArrayEquality(int[,] otherArray)
        {
            int rows = otherArray.GetLength(0);
            int columns = otherArray.GetLength(1);
            if (rows != _stateArray.GetLength(0) || columns != _stateArray.GetLength(1))
                return false;
            for(int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    if (otherArray[i, j] != _stateArray[i, j])
                        return false;
                }
            return true;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Puzzle8State && Equals((Puzzle8State)obj);
        }

        public override int GetHashCode()
        {
            return (_emptyStatePosition.GetHashCode() * 397) ^ _arrayHash;
        }

        private int CalculateArrayHash()
        {
            int arrayHash = 0;
            for (int i = 0; i < _stateArray.GetLength(0); i++)
                for (int j = 0; j < StateArray.GetLength(1); j++)
                {
                    arrayHash = arrayHash * 314159 + _stateArray[i, j];
                }
            return arrayHash;
        }
    }
}