using UGOAP.CommonUtils.FastName;

namespace UGOAP.KnowledgeRepresentation.Facts;

public record Facts
{
    public record Effects
    {
        public static FastName HasWood = new FastName("HasWood");
        public static FastName FireLit = new FastName("FireLit");
    }

    public record Preconditions
    {
        public static FastName HasWood = new FastName("HasWood");
    }

    public record Predicates
    {
        public static readonly FastName FireIsLit = new FastName("FireIsLit");
        public static readonly FastName FireIsNotLit = new FastName("FireIsNotLit");
        public static readonly FastName IsSitting = new FastName("IsSitting");
        public static readonly FastName IsInsideHouse = new FastName("IsInsideHouse");
        public static readonly FastName IsCovered = new FastName("IsCovered");
    }

    public record Goals
    {
        public static readonly FastName LitFire = new FastName("LitFire");
    }
}