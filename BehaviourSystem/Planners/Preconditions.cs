using System.Collections;
using System.Collections.Generic;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;

namespace UGOAP.BehaviourSystem.Planners;

public record Preconditions(HashSet<Belief> StatePreconditions) : IEnumerable<Belief>
{
    public Preconditions() : this(new HashSet<Belief>()) { }
    public void Add(Belief belief) => StatePreconditions.Add(belief);

    public IEnumerator<Belief> GetEnumerator()
    {
        return StatePreconditions.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}
