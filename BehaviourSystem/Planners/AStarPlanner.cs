using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public class AStarPlanner : BasePlanner
{
    private readonly IHeuristic _heuristic;

    public AStarPlanner(IHeuristic heuristic) => _heuristic = heuristic;

    public override Plan ComputePlan(HashSet<IAction> actions, Goal goal) => new Plan(new Queue<IAction>(), 0.0f);
    public override Plan ComputePlan(HashSet<IAction> actions, Goal goal, IState currentState)
    {
        var goalState = new State(goal.DesiredEffects);
        var originalState = currentState.Copy();
        if (IsGoalStateFulfilled(goalState, originalState))
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

            if (currentNode.IsFulfilled())
            {
                return ReconstructPath(currentNode);
            }

            var availableActions = new HashSet<IAction>(actions).OrderBy(a => a.ActionState.Cost());
            foreach (var action in availableActions)
            {
                if (action.ActionState.Effects.Any(e => e.FulfillsAnyRequiredEffects(currentNode.State)))
                {
                    var newState = currentNode.State.Copy();
                    action.ActionState.Effects.ForEach(e => e.ApplyEffect(newState));
                    AddPreconditionsToState(newState, action);
                    CompareAndUpdateStateWith(newState, originalState);
                    var newNode = new StatePlanNode(currentNode, action, newState, currentNode.Cost + action.ActionState.Cost());
                    if (!closedSet.Contains(newNode))
                    {
                        openSet.Enqueue(newNode, newNode.Cost + _heuristic.Compute(newNode));
                    }
                }

            }
        }
        GD.Print("No plan found");
        return new Plan(new Queue<IAction>(), 0.0f);
    }

    private bool IsGoalStateFulfilled(IState goalState, IState currentState)
    {
        foreach (var (predicate, belief) in goalState.BeliefComponent.Beliefs)
        {
            var currentBelief = currentState.BeliefComponent.GetBelief(predicate).Evaluate();
            var newBelief = goalState.BeliefComponent.GetBelief(predicate).Evaluate();
            var beliefsDontAgree = currentBelief != newBelief;
            if (beliefsDontAgree)
            {
                return false;
            }
        }
        return true;
    }

}
