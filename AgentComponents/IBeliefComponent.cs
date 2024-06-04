using System.Collections.Generic;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;

namespace UGOAP.AgentComponents;

public interface IBeliefComponent : ICopyable<IBeliefComponent>
{
    Dictionary<FastName, Belief> Beliefs { get; }
    void AddBelief(Belief belief);
    void UpdateBelief(Belief belief);
    void RemoveBelief(FastName predicate);
    Belief GetBelief(FastName predicate);
    void PrintBeliefs();
}