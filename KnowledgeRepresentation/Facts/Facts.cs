using UGOAP.CommonUtils.FastName;

namespace UGOAP.KnowledgeRepresentation.Facts;

public record Facts
{
    public record Effects
    {
        public static FastName HasWood = new FastName("HasWood");
        public static FastName FireLit = new FastName("FireLit");
        public static FastName HasMeat = new FastName("HasMeat");
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
        public static readonly FastName IsHungry = new FastName("IsHungry");
        public static readonly FastName NotHungry = new FastName("NotHungry");
        public static readonly FastName HasFood = new FastName("HasFood");
    }

    public record Goals
    {
        public static readonly FastName LitFire = new FastName("LitFire");
        public static readonly FastName Hunt = new FastName("Hunt");
        public static readonly FastName Eat = new FastName("Eat");
    }
}

public record Names
{
    public static readonly FastName Fire = new FastName("Fire");
    public static readonly FastName Health = new FastName("Health");
    public static readonly FastName Hunger = new FastName("Hunger");
}