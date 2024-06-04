using System;
using UGOAP.BehaviourSystem.Planners;

namespace UGOAP.AgentComponents.Interfaces;

public interface IPlanExecutioner
{
    event Action PlanFinished;
    void LoadPlan(Plan plan);
    void StopCurrentPlan();
}