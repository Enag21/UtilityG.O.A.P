using Godot;
using UGOAP.KnowledgeRepresentation.Facts;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public enum TraitType
{
    LikesRain,
    DislikesRain,
    None,
}

[GlobalClass]
public partial class Trait : Resource
{
    [Export] public TraitType Type { get; private set; }

    [Export(PropertyHint.Range, "1.0f, 10.0f")]
    public float Value { get; set; }
    public static Trait None => new Trait(TraitType.None, 0.0f);
    public Trait() { Type = TraitType.None; Value = 0.0f; }
    public Trait(TraitType type, float value) => (Type, Value) = (type, value);
}