using System;
using System.Linq;
using AIPlanning.Core;
using AIPlanning.Core.Interfaces;
using AIPlanning.Search;
using NUnit.Framework;
using Rhino.Mocks;

namespace AIPlanning.Tests.AStarSearch
{
    [TestFixture]
    public sealed class AStarTests
    {
        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullExcHeuristicTest()
        {
            new AStar<bool>(null);
        }

        [Test]
        public void FindGoalReachableTest()
        {
            var heuristic = MockRepository.GenerateStub<ICostFunction<bool>>();
            heuristic.Stub(h => h.CalculateCost(Arg<ISearchNode<bool>>.Is.Anything, Arg<IProblem<bool>>.Is.Anything))
                     .Return(0.0);
            var problemInitialState = MockRepository.GenerateStub<ISearchNode<bool>>();
            var goalState = MockRepository.GenerateStub<ISearchNode<bool>>();
            goalState.Stub(gs => gs.Expand())
                     .Return(Enumerable.Empty<ISearchNode<bool>>());
            problemInitialState.Stub(pis => pis.Expand())
                               .Return(new[] {goalState});
            var problem = MockRepository.GenerateStub<IProblem<bool>>();
            problem.Stub(p => p.InitialState)
                   .Return(problemInitialState);
            problem.Stub(p => p.GoalTest(Arg<ISearchNode<bool>>.Is.Anything))
                   .Do(new Func<ISearchNode<bool>, bool>(state => state == goalState));
            var astar = new AStar<bool>(heuristic);

            IPath<bool> path = astar.Find(problem);

            Assert.AreEqual(2, path.Steps.Count());
            Assert.AreEqual(1, path.Cost);
        }

        [Test]
        public void FindGoalUnreachableTest()
        {
            var heuristic = MockRepository.GenerateStub<ICostFunction<bool>>();
            heuristic.Stub(h => h.CalculateCost(Arg<ISearchNode<bool>>.Is.Anything, Arg<IProblem<bool>>.Is.Anything))
                     .Return(0.0);
            var problemInitialState = MockRepository.GenerateStub<ISearchNode<bool>>();
            problemInitialState.Stub(pis => pis.Expand())
                               .Return(Enumerable.Empty<ISearchNode<bool>>());
            var problem = MockRepository.GenerateStub<IProblem<bool>>();
            problem.Stub(p => p.InitialState)
                   .Return(problemInitialState);
            problem.Stub(p => p.GoalTest(Arg<ISearchNode<bool>>.Is.Anything))
                   .Return(false);
            var astar = new AStar<bool>(heuristic);

            IPath<bool> path = astar.Find(problem);

            Assert.AreEqual(Path<bool>.Empty, path);
        }

        [Test]
        [ExpectedException(typeof (ArgumentNullException))]
        public void FindProblemNullExcTest()
        {
            var heuristic = MockRepository.GenerateStub<ICostFunction<bool>>();
            var astar = new AStar<bool>(heuristic);
            astar.Find(null);
        }
    }
}