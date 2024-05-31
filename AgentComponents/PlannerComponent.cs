using System.Collections.Generic;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Planners;

namespace UGOAP.AgentComponents;

public class PlannerComponent
{
    readonly IPlanner _planner;
    public PlannerComponent(IPlanner planner) => _planner = planner;
    public Plan Plan(HashSet<IAction> actions, Goal goal) => _planner.ComputePlan(actions, goal);
}