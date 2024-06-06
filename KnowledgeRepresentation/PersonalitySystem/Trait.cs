using Godot;
using UGOAP.KnowledgeRepresentation.Facts;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public enum TraitType
{
    LikesRain,
    None,
}

[GlobalClass]
public partial class Trait : Resource
{
    [Export] public TraitType Type { get; private set; }

    [Export(PropertyHint.Range, "-1.0f, 1.0f")]
    public float Value { get; set; }

    public Trait(TraitType type, float value) => (Type, Value) = (type, value);
}