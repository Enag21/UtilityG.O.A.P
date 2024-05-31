namespace UGOAP.BehaviourSystem.Planners;

public interface IHeuristic
{
    float Compute(PlanNode planNode);
}

public class BeliefsHeuristic : IHeuristic
{
    public float Compute(PlanNode node) => node.RequiredEffects.Count;
}