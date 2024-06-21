using System.Collections.Generic;
using System.Linq;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.DecisionMakers;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public class UtilityPlanner : BasePlanner
{
    private readonly IUtilityRater _utilityRater;
    private readonly IHeuristic _heuristic;
    public UtilityPlanner(IUtilityRater utilityRater, IHeuristic heuristic) => (_utilityRater, _heuristic) = (utilityRater, heuristic);
    public override Plan ComputePlan(HashSet<IAction> actions, Goal goal) => new Plan(new Queue<IAction>(), 0.0f);

    public override Plan ComputePlan(HashSet<IAction> actions, Goal goal, IState currentState)
    {
        var goalState = new State(goal.DesiredEffects);
        var originalState = currentState.Copy();
        if (goal.GetSatisfaction(currentState) >= 0.95f)
        {
            return new Plan(new Queue<IAction>(), 0.0f);
        }
        InverseBeliefs(goalState);

        var openSet = new PriorityQueue<IPlanNode, float>();
        var closedSet = new HashSet<IPlanNode>();
        var rootNode = new StatePlanNode(null, null, goalState, 0.0f);

        openSet.Enqueue(rootNode, _heuristic.Compute(rootNode));

        while (openSet.Count > 0)
        {
            var currentNode = openSet.Dequeue() as StatePlanNode;
            closedSet.Add(currentNode);

            if (goal.GetSatisfaction(currentNode.State) >= 0.99f)
            {
                return ReconstructPath(currentNode);
            }

            var availableActions = new HashSet<IAction>(actions).OrderBy(a => a.ActionState.Cost());
            foreach (var action in availableActions)
            {
                if (!PreconditionsMet(action, currentNode.State)) continue;
                var newState = currentNode.State.Copy();
                action.ActionState.Effects.ForEach(effect => effect.ApplyEffect(newState));
                AddPreconditionsToState(newState, action);
                CompareAndUpdateStateWith(newState, originalState);
                var newNode = new StatePlanNode(currentNode, action, newState, currentNode.Cost + action.ActionState.Cost());
                if (!closedSet.Contains(newNode))
                {
                    openSet.Enqueue(newNode, -goal.GetSatisfaction(newNode.State));
                }
            }
        }
        GD.Print("No plan found");
        return new Plan(new Queue<IAction>(), 0.0f);
    }

    private bool PreconditionsMet(IAction action, IState state)
    {
        foreach (var precondition in action.ActionState.Preconditions)
        {
            if (state.BeliefComponent.GetBelief(precondition.Predicate).Evaluate() != precondition.Evaluate())
            {
                return false;
            }
        }
        return true;
    }
}