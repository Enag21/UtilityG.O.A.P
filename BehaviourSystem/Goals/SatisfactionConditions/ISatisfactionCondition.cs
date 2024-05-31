using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Goals.SatisfactionConditions;

public interface ISatisfactionCondition
{
    float GetSatisfaction(IState state);
}