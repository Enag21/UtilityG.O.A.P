using System.Collections.Generic;
using System.Linq;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public abstract class BasePlanner : IPlanner
{
    public abstract Plan ComputePlan(HashSet<IAction> actions, Goal goal, IState currentState);
    public abstract Plan ComputePlan(HashSet<IAction> actions, Goal goal);

    protected void AddPreconditionsToState(IState state, IAction action)
    {
        foreach (var precondition in action.ActionState.Preconditions)
        {
            var preconditionInversed = precondition.Copy().Invert();
            state.BeliefComponent.UpdateBelief(preconditionInversed);
        }
    }

    protected void InverseBeliefs(IState goalState)
    {
        foreach (var (predicate, belief) in goalState.BeliefComponent.Beliefs)
        {
            belief.Invert();
        }
    }


    protected void CompareAndUpdateStateWith(IState stateToUpdate, IState stateToCompare)
    {
        var trueBeliefs = stateToCompare.BeliefComponent.Beliefs.Where(b => b.Value.Evaluate()).Select(b => b.Value);
        var beliefsToUpdate = stateToUpdate.BeliefComponent.Beliefs;
        foreach (var belief in trueBeliefs)
        {
            foreach (var (predicate, beliefToUpdate) in beliefsToUpdate)
            {
                if (predicate == belief.Predicate)
                {
                    stateToUpdate.BeliefComponent.UpdateBelief(belief);
                }
            }
        }
    }

    protected static Plan ReconstructPath(IPlanNode currentNode)
    {
        IState finalState = null;
        if (currentNode is StatePlanNode statePlanNode)
        {
            finalState = statePlanNode.State;
        }

        var actions = new Queue<IAction>();
        var totalCost = currentNode.Cost;
        while (currentNode.Parent != null)
        {
            actions.Enqueue(currentNode.Action);
            currentNode = currentNode.Parent;
        }

        return new Plan(actions, totalCost, finalState);
    }
}
