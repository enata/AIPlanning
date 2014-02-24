using AIPlanning.Graphplan;
using AIPlanning.PddlParsing;
using System;
using System.IO;

namespace AIPlanning
{
    class Program
    {
        static void Main(string[] args)
        {
            var domainStr = File.ReadAllText("dwr-operators-pos.txt");
            var strProblem = File.ReadAllText("dwr-problem1-pos.txt");
            var domainBuilder = new PddlModelBuilder(domainStr);
            var domain = domainBuilder.Domain;
            var problemBuilder = new PddlModelBuilder(strProblem);
            var problem = problemBuilder.Problem;
            var solutionSearcher = new ProblemSolutionSearcher(domain, problem);
            var graph = solutionSearcher.Graph;
            Console.WriteLine("!");
        }
    }
}
