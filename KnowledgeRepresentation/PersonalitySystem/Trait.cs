using Godot;

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
    [Export]public TraitType Type { get; private set; }
    [Export] public float Value { get; set; }
    public static Trait None { get; } = new Trait(TraitType.None, 0.0f);

    public Trait() { }

    public Trait(TraitType type, float value)
    {
        Type = type;
        Value = value;
    }
}
