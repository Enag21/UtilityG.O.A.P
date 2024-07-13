

using Godot.Collections;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public interface ITraitManager
{
    Array<TraitResource> Traits{ get; }
    void AddTrait(TraitType type, float value);
    void RemoveTrait(TraitType type);
    Trait GetTrait(TraitType type);
}