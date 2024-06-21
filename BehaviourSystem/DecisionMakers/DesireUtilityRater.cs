using System.Collections.Generic;
using Godot;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.DecisionMakers;

public class DesireUtilityRater : IUtilityRater
{
    private readonly List<Desires.Desire> _desires;

    public DesireUtilityRater(List<Desires.Desire> desires) => _desires = desires;

    public float RateUtility(IState state)
    {
        var valueSum = 0.0f;
        var weightSum = 0.0f;
        foreach (var desire in _desires)
        {
            valueSum += desire.Weight * desire.ComputeSatisfaction(state);
            weightSum += desire.Weight;
        }
        return valueSum / weightSum;
    }
}