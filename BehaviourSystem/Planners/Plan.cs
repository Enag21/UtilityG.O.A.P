using System.Collections.Generic;
using System.Linq;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public record Plan(Queue<IAction> Actions, float TotalCost, IState State = null);

public interface IPlanNode
{
    IPlanNode Parent { get; }
    IAction Action { get; }
    float Cost { get; }
    bool IsFulfilled();
    int GetUnfulfilledConditionsCount();
}
public record PlanNode(IPlanNode Parent, IAction Action, Effects RequiredEffects, float Cost) : IPlanNode
{
    public int GetUnfulfilledConditionsCount()
    {
        return RequiredEffects.Count;
    }

    public bool IsFulfilled()
    {
        return RequiredEffects.Count == 0;
    }
}

public record StatePlanNode(IPlanNode Parent, IAction Action, IState State, float Cost) : IPlanNode
{
    public int GetUnfulfilledConditionsCount()
    {
        var unfulfilledBelief = State.BeliefComponent.Beliefs
            .Where(b => !b.Value.Evaluate())
            .Select(b => b.Key)
            .ToList();
        return unfulfilledBelief.Count;
    }

    public bool IsFulfilled()
    {
        foreach (var (predicate, belief) in State.BeliefComponent.Beliefs)
        {
            if (!belief.Evaluate())
            {
                return false;
            }
        }
        return true;
    }
}