using System.Collections.Generic;
using System.Linq;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;

namespace UGOAP.BehaviourSystem.Planners;

public class AStarPlanner : IPlanner
{
    private readonly IHeuristic _heuristic;

    public AStarPlanner(IHeuristic heuristic) => _heuristic = heuristic;

    public Plan ComputePlan(HashSet<IAction> actions, Goal goal)
    {
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
        GD.Print("No plan found");
        return null;
    }

    private static Plan ReconstructPath(PlanNode currentNode)
    {
        var actions = new Queue<IAction>();
        var totalCost = currentNode.Cost;
        while (currentNode.Parent != null)
        {
            actions.Enqueue(currentNode.Action);
            currentNode = currentNode.Parent;
        }

        return new Plan(actions, totalCost);
    }
}