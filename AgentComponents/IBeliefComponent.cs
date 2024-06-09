using System.Collections.Generic;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;

namespace UGOAP.AgentComponents;

public interface IBeliefComponent : ICopyable<IBeliefComponent>
{
    Dictionary<CommonUtils.FastName.FastName, Belief> Beliefs { get; }
    void AddBelief(Belief belief);
    void UpdateBelief(Belief belief);
    void RemoveBelief(CommonUtils.FastName.FastName predicate);
    Belief GetBelief(CommonUtils.FastName.FastName predicate);
    void PrintBeliefs();
}