using System.Collections.Generic;
using System.Linq;
using UGOAP.CommonUtils.ExtensionMethods;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public class Effects
{
    public HashSet<FastName> SimpleEffects { get; } = new();
    public HashSet<Belief> StateEffects { get; } = new();

    public int Count => SimpleEffects.Count + StateEffects.Count;

    public Effects() { }

    public Effects(Effects other)
    {
        SimpleEffects = new HashSet<FastName>(other.SimpleEffects);
        StateEffects = new HashSet<Belief>(other.StateEffects);
    }

    public void AddPreconditions(Preconditions preconditions)
    {
        SimpleEffects.UnionWith(preconditions.SimplePreconditions);
        StateEffects.UnionWith(preconditions.StatePreconditions);
    }

    public void ExceptWithFulfilledStateEffects()
    {
        StateEffects.RemoveWhere(effect => effect.Evaluate());
    }

    public void ExceptWithEffects(Effects effectsToRemove)
    {
        SimpleEffects.RemoveWhere(effect => effectsToRemove.SimpleEffects.Contains(effect));
    }

    public void Add(FastName efffect)
    {
        SimpleEffects.Add(efffect);
    }

    public void Add(Belief stateEffect)
    {
        StateEffects.Add(stateEffect);
    }

    public void ApplyEffects(IState state)
    {
        StateEffects.ForEach(effect => state.BeliefComponent.UpdateBelief(effect));
    }

    public bool FulfillsAnyRequiredEffects(Effects requiredEffects)
    {
        var simpleEffects = false;
        foreach (var effect in requiredEffects.SimpleEffects)
        {
            foreach (var simpleEffect in SimpleEffects)
            {
                if (effect == simpleEffect)
                {
                    return true;
                }
            }
        }
        return simpleEffects;
    }
}
