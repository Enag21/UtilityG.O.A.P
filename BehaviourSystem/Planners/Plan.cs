using System.Collections.Generic;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Goals;

namespace UGOAP.BehaviourSystem.Planners;

public record Plan(Queue<IAction> Actions, float TotalCost, Goal Goal = null);

public record PlanNode(PlanNode Parent, IAction Action, Effects RequiredEffects, float Cost);
