using UGOAP.CommonUtils.FastName;

namespace UGOAP.KnowledgeRepresentation.Facts;

public record Facts
{
    public record Effects
    {
        public static FastName HasWood = new FastName("HasWood");
        public static  FastName FireLit = new FastName("FireLit");
    }

    public record Preconditions
    {
        public static FastName HasWood = new FastName("HasWood");
    }

    public record Predicates
    {
        public static FastName FireIsLit = new FastName("FireIsLit");
        public static FastName FireIsNotLit = new FastName("FireIsNotLit");
        public static readonly FastName IsSitting = new FastName("IsSitting");
    }

    public record Goals
    {
        public static readonly FastName LitFire = new FastName("LitFire");
    }
}