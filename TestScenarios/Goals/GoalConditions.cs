using System;
using UGOAP.BehaviourSystem.Goals.SatisfactionConditions;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP;

public class FireLitCondition : ISatisfactionCondition
{
    private readonly FirePit _firePit;
    public FireLitCondition(FirePit firePit) => _firePit = firePit;
    public float GetSatisfaction(IState state)
    {
        if (_firePit.IsLit)
        {
            return 1.0f;
        }
        return 0.0f;
    }
}
