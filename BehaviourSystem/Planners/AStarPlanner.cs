using System.Collections.Generic;
using System.Linq;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public class AStarPlanner : IPlanner
{
    private readonly IHeuristic _heuristic;

    public AStarPlanner(IHeuristic heuristic) => _heuristic = heuristic;

    public Plan ComputePlan(HashSet<IAction> actions, Goal goal)
    {
    /*
        var openSet = new PriorityQueue<PlanNode, float>();
        var closedSet = new HashSet<PlanNode>();
        var rootNode = new PlanNode(null, null, goal.DesiredEffects, 0);

        openSet.Enqueue(rootNode, _heuristic.Compute(rootNode));

        while (openSet.Count > 0)
        {
            var currentNode = openSet.Dequeue();
            closedSet.Add(currentNode);

            if (currentNode.RequiredEffects.Count == 0)
            {
                return ReconstructPath(currentNode);
            }

            var availableActions = new HashSet<IAction>(actions).OrderBy(a => a.Cost);
            foreach (var action in availableActions)
            {
                if (!action.Effects.FulfillsAnyRequiredEffects(currentNode.RequiredEffects)) continue;
                var newRequiredEffects = new Effects(currentNode.RequiredEffects);
                newRequiredEffects.ExceptWithEffects(action.Effects);
                newRequiredEffects.AddPreconditions(action.Preconditions);

                var newNode = new PlanNode(
                    currentNode,
                    action,
                    newRequiredEffects,
                    currentNode.Cost + action.Cost
                );

                if (!closedSet.Contains(newNode))
                {
                    openSet.Enqueue(newNode, newNode.Cost + _heuristic.Compute(newNode));
                }
            }
        }
      */
      GD.Print("No plan found");
      return null;
    }

    private static Plan ReconstructPath(IPlanNode currentNode)
    {
        IState finalState = null;
        if (currentNode is StatePlanNode statePlanNode)
        {
            finalState = statePlanNode.State;
            finalState.BeliefComponent.PrintBeliefs();
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

    public Plan ComputePlan(HashSet<IAction> actions, Goal goal, IState currentState)
    {
        var goalState =  new State(goal.DesiredEffects.StateEffects);
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

            var availableActions = new HashSet<IAction>(actions).OrderBy(a => a.Cost);
            foreach (var action in availableActions)
            {
                if (!action.Effects.FulfillsAnyRequiredEffects(currentNode.State)) continue;

                var newState = currentNode.State.Copy();
                action.Effects.ApplyEffects(newState);
                AddPreconditionsToState(newState, action);
                CompareAndUpdateStateWith(newState, originalState);
                var newNode = new StatePlanNode(currentNode, action, newState, currentNode.Cost + action.Cost);
                if (!closedSet.Contains(newNode))
                {
                    openSet.Enqueue(newNode, newNode.Cost + _heuristic.Compute(newNode));
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
            var currentBelief = currentState.BeliefComponent.GetBelief(predicate);
            var beliefsDontAgree = currentBelief.Evaluate() != belief.Evaluate();
            if (beliefsDontAgree)
            {
                return false;
            }
        }
        return true;
    }

    private void InverseBeliefs(IState goalState)
    {
        foreach (var (predicate, belief) in goalState.BeliefComponent.Beliefs)
        {
            belief.Invert();
        }
    }

    private void AddPreconditionsToState(IState state, IAction action)
    {
        foreach (var precondition in action.Preconditions)
        {
            var preconditionInversed = precondition.Copy().Invert();
            state.BeliefComponent.AddBelief(preconditionInversed);
        }
    }

    private void CompareAndUpdateStateWith(IState stateToUpdate, IState stateToCompare)
    {
        var trueBeliefs = stateToCompare.BeliefComponent.Beliefs.Where(b => b.Value.Evaluate()).Select(b => b.Value);
        var beliefsToUpdate = stateToUpdate.BeliefComponent.Beliefs;
        foreach (var belief in trueBeliefs)
        {
            foreach(var (predicate, beliefToUpdate) in beliefsToUpdate)
            {
                if (predicate == belief.Predicate)
                {
                    stateToUpdate.BeliefComponent.UpdateBelief(belief);
                }
            }
        }
    }
}