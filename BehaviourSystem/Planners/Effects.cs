using System.Collections.Generic;
using System.Linq;
using UGOAP.CommonUtils.ExtensionMethods;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public class Effects
{
    public HashSet<Belief> StateEffects { get; } = new();

    public int Count => StateEffects.Count;

    public Effects() { }

    public Effects(Effects other)
    {
        StateEffects = new HashSet<Belief>(other.StateEffects);
    }

    public void ExceptWithFulfilledStateEffects()
    {
        StateEffects.RemoveWhere(effect => effect.Evaluate());
    }

    public void Add(Belief stateEffect)
    {
        StateEffects.Add(stateEffect);
    }

    public void ApplyEffects(IState state)
    {
        StateEffects.ForEach(effect => state.BeliefComponent.UpdateBelief(effect));
    }

    public bool FulfillsAnyRequiredEffects(IState state)
    {
        var unfuldilledEffects = state.BeliefComponent.Beliefs.Where(b => !b.Value.Evaluate());
        foreach (var (predicate, belief) in unfuldilledEffects)
        {
            if (StateEffects.Any(effect => effect.Predicate == predicate))
            {
                return true;
            }
        }
        return false;
    }
}
