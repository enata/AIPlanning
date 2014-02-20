using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Pddl
{
    public sealed class PddlProblem
    {
        private readonly string _name;
        private readonly PddlVariable[] _objects;
        private readonly PddlPredicateCall[] _pddlStatements;
        private readonly PddlLogicOperation _goal;
        private readonly string _domain;

        public PddlProblem(string name, IEnumerable<PddlVariable> objects, IEnumerable<PddlPredicateCall> pddlStatements, PddlLogicOperation goal, string domain)
        {
            if (objects == null) throw new ArgumentNullException("objects");
            if (pddlStatements == null) throw new ArgumentNullException("pddlStatements");
            if (goal == null) throw new ArgumentNullException("goal");
            if(string.IsNullOrEmpty(name)) throw new ArgumentException();
            if (string.IsNullOrEmpty(domain)) throw new ArgumentException();

            _name = name;
            _goal = goal;
            _domain = domain;
            _pddlStatements = pddlStatements.ToArray();
            _objects = objects.ToArray();
        }

        public string Name
        {
            get { return _name; }
        }

        public IEnumerable<PddlVariable> Objects
        {
            get { return _objects; }
        }

        public IEnumerable<PddlPredicateCall> PddlStatements
        {
            get { return _pddlStatements; }
        }

        public PddlLogicOperation Goal
        {
            get { return _goal; }
        }

        public string Domain
        {
            get { return _domain; }
        }
    }
}