using Godot;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

[GlobalClass]
public partial class TraitResource : Resource
{
    [Export]
    public TraitType Type { get; private set; }

    [Export]
    public float Value { get; set; }

    public TraitResource() { }

    public TraitResource(TraitType type, float value)
    {
        Type = type;
        Value = value;
    }

    public Trait GetTrait() => new Trait(Type, Value);
}
