using System.Collections.Generic;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public interface ITraitManager
{
    Dictionary<TraitType, Trait> Traits { get; }

    void AddTrait(TraitType type, float value);
    void RemoveTrait(TraitType type);
    Trait GetTrait(TraitType type);
}