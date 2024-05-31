using System.Collections.Generic;
using UGOAP.KnowledgeRepresentation.BeliefSystem;

namespace UGOAP.CommonUtils.ExtensionMethods;

public static class BeliefExtensions
{
    public static void ExceptWithEffects(
        this HashSet<FastName.FastName> effects,
        HashSet<FastName.FastName> effectsToRemove
    )
    {
        foreach (var toRemove in effectsToRemove)
        {
            effects.RemoveWhere(effect => effect == toRemove);
        }
    }

    public static void ExceptWithStateEffects(
        this HashSet<Belief> effects,
        HashSet<Belief> effectsToRemove
    )
    {
        foreach (var toRemove in effectsToRemove)
        {
            effects.RemoveWhere(effect => effect.Predicate == toRemove.Predicate);
        }
    }
}