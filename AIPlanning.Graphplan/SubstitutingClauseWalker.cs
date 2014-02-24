using System.Collections.Generic;

namespace AIPlanning.Graphplan
{
    internal sealed class SubstitutingClauseWalker : ClauseWalker
    {
        private readonly Dictionary<string, string> _substitutions;

        public SubstitutingClauseWalker(Dictionary<string, string> substitutions)
        {
            _substitutions = substitutions;
        }

        protected override string SubstituteParameter(string parameter)
        {
            return _substitutions[parameter];
        }
    }
}