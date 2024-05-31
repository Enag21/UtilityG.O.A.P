using System.Collections.Generic;
using UGOAP.BehaviourSystem.Planners;

namespace UGOAP.BehaviourSystem.DecisionMakers;

public interface IDecisionMaker
{
    Plan Decide(HashSet<Plan> plans);
}