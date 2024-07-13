using System.Collections.Generic;
using Godot;
using System.Linq;
using Godot.Collections;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

[GlobalClass]
public partial class TraitManager : Node, ITraitManager
{
    [Export] public Array<TraitResource> Traits { get; set; }

    private List<Trait> _traits;

    public override void _Ready()
    {
        _traits = new List<Trait>();
        foreach (var trait in Traits)
        {
            _traits.Add(trait.GetTrait());
        }
    }

    public TraitManager() { }

    public void AddTrait(TraitType type, float value) => _traits.Add(new Trait(type, value));

    public void RemoveTrait(TraitType type)
    {
        var trait = _traits.FirstOrDefault(t => t.Type == type);
        _traits.Remove(trait);
    }

    public Trait GetTrait(TraitType type)
    {
        var trait = _traits.FirstOrDefault(t => t.Type == type);
        if (trait != null)
            return trait;
        return new NoneTrait();
    }
}