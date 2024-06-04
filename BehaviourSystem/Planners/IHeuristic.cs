namespace UGOAP.BehaviourSystem.Planners;

public interface IHeuristic
{
    float Compute(IPlanNode planNode);
}

public class BeliefsHeuristic : IHeuristic
{
    public float Compute(IPlanNode node) => node.GetUnfulfilledConditionsCount();
}