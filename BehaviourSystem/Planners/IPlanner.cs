using System.Collections.Generic;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;

namespace UGOAP.BehaviourSystem.Planners;

public interface IPlanner
{
    Plan ComputePlan(HashSet<IAction> actions, Goal goal);
}