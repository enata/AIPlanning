using System;
using System.Collections.Generic;
using System.Linq;
using AIPlanning.Core.Interfaces;
using AIPlanning.Search;
using AIPlanning.Search.Problems.Puzzle8;
using NUnit.Framework;

namespace AIPlanning.Test.Search.Problems
{
    [TestFixture]
    public sealed class Puzzle8Test
    {
        [Test]
        public void Puzzle8ProblemTest()
        {
            var goalState = new Puzzle8State(new[,] {{3, 2, 1}, {0, 4, 5}, {6, 7, 8}}, new Tuple<int, int>(1, 0));
            var initialState = new Puzzle8State(new[,] {{3, 7, 1}, {8, 0, 6}, {4, 2, 5}}, new Tuple<int, int>(1, 1));
            var problem = new Puzzle8Problem(goalState, initialState);
            var heuristic = new Puzzle8NoHeurisitcs();
            var searcher = new AStar<Puzzle8State>(heuristic);
            List<ISearchNode<Puzzle8State>> searchResult = searcher.Find(problem)
                                                                   .Steps.ToList();
            Assert.AreEqual(20, searchResult.Count);
        }

        [Test]
        public void Puzzle8BranchingFactorTest()
        {
            var initialState = new Puzzle8State(new[,] { { 0, 1, 2 }, { 3, 4, 5 }, { 6, 7, 8 } }, new Tuple<int, int>(0, 0));
            var heuristic = new Puzzle8NoHeurisitcs();
            var searcher = new AStar<Puzzle8State>(heuristic);
            int statesNumber = searcher.CountStatesAwayFrom(new Puzzle8Node(initialState), 15);
            Assert.AreEqual(2512, statesNumber);
        }
    }
}