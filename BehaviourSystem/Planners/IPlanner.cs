using System.Collections.Generic;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public interface IPlanner
{
    Plan ComputePlan(HashSet<IAction> actions, Goal goal);
    Plan ComputePlan(HashSet<IAction> actions, Goal goal, IState currentState);
}