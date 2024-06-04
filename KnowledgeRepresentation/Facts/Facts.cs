using UGOAP.CommonUtils.FastName;

namespace UGOAP.KnowledgeRepresentation.Facts;

public record Facts
{
    public record Effects
    {
        public static FastName HasWood => AllNames.HasWood;
        public static  FastName FireLit => AllNames.FireLit;
    }

    public record Preconditions
    {
        public static FastName HasWood => AllNames.HasWood;
    }

    public record Predicates
    {
        public static FastName FireIsLit => AllNames.FireIslit;
        public static FastName FireIsNotLit => AllNames.FireIsNotLit;
        public static readonly FastName IsSitting = new FastName("IsSitting");
    }

    public record Goals
    {
        public static readonly FastName LitFire = new FastName("LitFire");
    }

    private record AllNames
    {
        public static readonly FastName HasWood = new FastName("HasWood");
        public static readonly FastName FireIslit = new FastName("FireIslit");
        public static readonly FastName FireIsNotLit = new FastName("FireIsNotLit");
        public static readonly FastName LitFire = new FastName("LitFire");
        public static readonly FastName FireLit = new FastName("FireLit");
    }
}