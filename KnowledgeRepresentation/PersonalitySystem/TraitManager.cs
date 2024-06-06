using Godot;
using Godot.Collections;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

[GlobalClass]
public partial class TraitManager : Node, ITraitManager
{
    [Export] public Array<Trait> Traits { get; set; }

    public TraitManager() { }

    public void AddTrait(TraitType type, float value) => Traits.Add(new Trait(type, value));

    public void RemoveTrait(TraitType type)
    {
        foreach (Trait trait in Traits)
        {
            if (trait.Type == type)
            {
                Traits.Remove(trait);
                break;
            }
        }
    }

    public Trait GetTrait(TraitType type)
    {
        foreach (Trait trait in Traits)
        {
            if (trait.Type == type)
            {
                return trait;
            }
        }
        return new Trait(TraitType.None, 0);
    }
}