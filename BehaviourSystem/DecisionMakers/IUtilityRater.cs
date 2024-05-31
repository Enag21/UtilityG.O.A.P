using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.DecisionMakers;

public interface IUtilityRater
{
    float RateUtility(IState state);
}