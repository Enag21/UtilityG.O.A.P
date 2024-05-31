using System.Collections.Generic;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.AgentComponents;

public class PlannerComponent
{
    readonly IPlanner _planner;
    public PlannerComponent(IPlanner planner) => _planner = planner;
    public Plan Plan(HashSet<IAction> actions, Goal goal) => _planner.ComputePlan(actions, goal);
    public Plan Plan(HashSet<IAction> actions, Goal goal, IState currentState) => _planner.ComputePlan(actions, goal, currentState);
}