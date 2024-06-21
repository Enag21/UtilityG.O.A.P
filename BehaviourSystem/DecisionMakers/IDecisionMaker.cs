using System.Collections.Generic;
using UGOAP.BehaviourSystem.Planners;

namespace UGOAP.BehaviourSystem.DecisionMakers;

public interface IDecisionMaker
{
    IUtilityRater UtilityRater { get; }
    Plan Decide(HashSet<Plan> plans);
    void ReComputeUtilityForPlan(Plan currentPlan);
}