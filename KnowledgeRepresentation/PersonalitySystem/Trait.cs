namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public enum TraitType
{
    None = 0,
    Good = 1,
    Bad = 2
}

public class Trait
{
    public TraitType Type { get; }
    public float Value { get; }

    public Trait(TraitType type, float value) => (Type, Value) = (type, value);
}