using System.Collections.Generic;
using UGOAP.BehaviourSystem.Goals;

namespace UGOAP.AgentComponents.Interfaces;

public interface IGoalDriver
{
    public List<Goal> ActiveGoals { get; }
}