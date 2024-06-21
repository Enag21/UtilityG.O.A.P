using System.Collections.Generic;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.SmartObjects;
using UGOAP.CommonUtils.FastName;

namespace UGOAP.AgentComponents;

public interface IBeliefComponent : ICopyable<IBeliefComponent>
{
    Dictionary<FastName, Belief> Beliefs { get; }
    void AddBelief(Belief belief);
    void UpdateBelief(Belief belief);
    void RemoveBelief(FastName predicate);
    Belief GetBelief(FastName predicate);
    Belief GetBeliefAboutEntity(IEntity entity);
    List<Belief> GetSpecificEntityBeliefs<T>() where T : class;
    void PrintBeliefs();
}