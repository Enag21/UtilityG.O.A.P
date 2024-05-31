using UGOAP.CommonUtils.FastName;

namespace UGOAP.KnowledgeRepresentation.Facts;

public static class Facts
{
    public class Effects
    {
        public static FastName HasWood => AllNames.HasWood;
        public static  FastName FireLit => AllNames.FireLit;
    }

    public class Preconditions
    {
        public static FastName HasWood => AllNames.HasWood;
    }

    public class Predicates
    {
        public static FastName FireIsLit => AllNames.FireIslit;
        public static FastName FireIsNotLit => AllNames.FireIsNotLit;
    }

    public class Goals
    {
        public static readonly FastName LitFire = new FastName("LitFire");
    }

    private class AllNames
    {
        public static readonly FastName HasWood = new FastName("HasWood");
        public static readonly FastName FireIslit = new FastName("FireIslit");
        public static readonly FastName FireIsNotLit = new FastName("FireIsNotLit");
        public static readonly FastName LitFire = new FastName("LitFire");
        public static readonly FastName FireLit = new FastName("FireLit");
    }
}