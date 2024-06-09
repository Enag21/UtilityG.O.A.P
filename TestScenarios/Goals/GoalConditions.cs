using UGOAP.BehaviourSystem.Goals.SatisfactionConditions;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.TestScenarios.Goals;

public class FireLitCondition : ISatisfactionCondition
{
    public float GetSatisfaction(IState state)
    {
        if (state.BeliefComponent.GetBelief(Facts.Predicates.FireIsLit).Evaluate())
        {
            return 1.0f;
        }
        return 0.0f;
    }
}
