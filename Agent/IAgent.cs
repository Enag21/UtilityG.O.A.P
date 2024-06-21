using System.Collections.Generic;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.SmartObjects;

namespace UGOAP.Agent;

public interface IAgent : ISmartObject
{
    IState State { get; }
    INavigationComponent NavigationComponent { get; }
    IGoalDriver GoalDriver { get; }
}