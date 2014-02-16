using AIPlanning.Search;
using AIPlanning.Search.Problems.Puzzle8;
using System;

namespace AIPlanning
{
    class Program
    {
        static void Main(string[] args)
        {
            var initialState = new Puzzle8State(new[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } });
            var heuristic = new Puzzle8NoHeurisitcs();
            var searcher = new AStar<Puzzle8State>(heuristic);
            int statesNumber = searcher.CountStatesAwayFrom(new Puzzle8Node(initialState), 27);
            Console.WriteLine("!");
        }
    }
}
