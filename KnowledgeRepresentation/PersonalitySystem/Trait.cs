using Godot;
using Godot.NativeInterop;
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
    public TraitType Type { get; private set; }
    public float Value { get; set; }
    public static Trait None { get; } = new Trait(TraitType.None, 0.0f);

    public Trait() { }

    public Trait(TraitType type, float value)
    {
        Type = type;
        Value = value;
    }
}
