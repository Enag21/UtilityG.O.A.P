namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public enum TraitType
{
    LikesRain,
    DislikesRain,
    None,
}

public record Trait(TraitType Type, float Value);

public record NoneTrait() : Trait(TraitType.None, 0.0f);
