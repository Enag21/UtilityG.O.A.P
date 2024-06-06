using System.Collections.Generic;
using System.Net;
using Godot;
using UGOAP.BehaviourSystem.Desires;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Goals.SatisfactionConditions;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP;

[GlobalClass]
public partial class DesireToReactToRain : Desire
{
    protected override void ConfigureTriggers()
    {
        if (_agent.State.TraitManager.GetTrait(TraitType.LikesRain) != Trait.None)
        {
            // noop
            // TODO: Do something differnet if the agent likes the rain
        }
        if (_agent.State.TraitManager.GetTrait(TraitType.DislikesRain) != Trait.None)
        {
            var trigger = new Belief.BeliefBuilder(new FastName("IsRaining"))
                .WithCondition(() => WeatherComponent.Instance.CurrentWeather == WeatherType.Rain).Build();
            var goal = new Goal.Builder(new FastName("ReactToRain"))
                .WithPriority(20.0f)
                .WithSatisfactionCondition(new IsCoveredCondition())
                .WithDesiredEffect(new Belief.BeliefBuilder(Facts.Predicates.IsCovered).WithCondition(() => true).Build())
                .Build();
            triggerMapping.Add(trigger, new List<Goal>() { goal });
        }
    }
}

public class IsCoveredCondition : ISatisfactionCondition
{
    public float GetSatisfaction(IState state)
    {
        if (state.BeliefComponent.GetBelief(Facts.Predicates.IsCovered).Evaluate())
        {
            return 1.0f;
        }
        return 0.0f;
    }
}