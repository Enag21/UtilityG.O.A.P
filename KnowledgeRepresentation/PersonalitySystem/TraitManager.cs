using System.Collections.Generic;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public class TraitManager : ITraitManager
{
    public Dictionary<TraitType, Trait> Traits { get; } = new();

    public TraitManager() { }

    public void AddTrait(TraitType type, float value) => Traits.Add(type, new Trait(type, value));

    public void RemoveTrait(TraitType type) => Traits.Remove(type);

    public Trait GetTrait(TraitType type)
    {
        if (Traits.TryGetValue(type, out Trait trait))
        {
            return trait;
        }
        return new Trait(TraitType.None, 0);
    }
}