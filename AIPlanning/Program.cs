using AIPlanning.PddlParsing;
using System;
using System.IO;

namespace AIPlanning
{
    class Program
    {
        static void Main(string[] args)
        {
            var str = File.ReadAllText("dwr-problem1-pos.txt");
            var builder = new PddlModelBuilder(str);
            Console.WriteLine("!");
        }
    }
}
