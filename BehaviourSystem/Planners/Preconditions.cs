using System.Collections.Generic;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;

namespace UGOAP.BehaviourSystem.Planners;

public record Preconditions(
    HashSet<FastName> SimplePreconditions,
    HashSet<Belief> StatePreconditions
)
{
    public void Add(FastName precondition) => SimplePreconditions.Add(precondition);

    public void Add(Belief belief) => StatePreconditions.Add(belief);
}
