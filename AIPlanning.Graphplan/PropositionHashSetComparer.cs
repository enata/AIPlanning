using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Graphplan
{
    internal sealed class PropositionHashSetComparer : IEqualityComparer<HashSet<PlanningGraph.Proposition>>
    {
        public bool Equals(HashSet<PlanningGraph.Proposition> x, HashSet<PlanningGraph.Proposition> y)
        {
            return x.SetEquals(y);
        }

        public int GetHashCode(HashSet<PlanningGraph.Proposition> obj)
        {
            int result = obj.Aggregate(0, (current, proposition) => current*397 + obj.GetHashCode());
            return result;
        }
    }
}