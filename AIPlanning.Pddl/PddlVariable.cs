using System;

namespace AIPlanning.Pddl
{
    public sealed class PddlVariable
    {
        private readonly string _type;
        private readonly string _name;

        public PddlVariable(string type, string name)
        {
            if (string.IsNullOrEmpty(type)) throw new ArgumentException("type");
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("name");

            _type = type;
            _name = name;
        }

        public string Type
        {
            get { return _type; }
        }

        public string Name
        {
            get { return _name; }
        }
    }
}