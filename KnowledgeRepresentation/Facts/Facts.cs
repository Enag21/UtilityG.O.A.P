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
        public static FastName FireIsUnlit => AllNames.FireIsUnlit;
    }

    public class Goals
    {
        public static readonly FastName LitFire = new FastName("LitFire");
    }

    private class AllNames
    {
        public static readonly FastName HasWood = new FastName("HasWood");
        public static readonly FastName FireIsUnlit = new FastName("FireIsUnlit");
        public static readonly FastName LitFire = new FastName("LitFire");
        public static readonly FastName FireLit = new FastName("FireLit");
    }
}