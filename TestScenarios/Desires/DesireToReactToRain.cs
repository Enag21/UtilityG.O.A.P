using Godot;
using UGOAP.BehaviourSystem.Desires;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Goals.SatisfactionConditions;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.TestScenarios.Systems.WeatherSystem;

namespace UGOAP.TestScenarios.Desires;

[GlobalClass]
public partial class DesireToReactToRain : Desire
{
    protected override void ConfigureTriggers()
    {
        if (Agent.State.TraitManager.GetTrait(TraitType.LikesRain) != Trait.None)
        {
            // noop
            // TODO: Do something differnet if the agent likes the rain
        }
        if (Agent.State.TraitManager.GetTrait(TraitType.DislikesRain) != Trait.None)
        {
            var trigger = new Belief.BeliefBuilder(new CommonUtils.FastName.FastName("IsRaining"))
                .WithCondition(() => TriggerCondition()).Build();
            var goal = () => new Goal.Builder(new CommonUtils.FastName.FastName("ReactToRain"))
                .WithPriority(20.0f)
                .WithSatisfactionCondition(new IsCoveredCondition())
                .WithDesiredEffect(new Belief.BeliefBuilder(Facts.Predicates.IsCovered).WithCondition(() => true).Build())
                .Build();
            var triggerMap = new TriggerGoalMap(trigger);
            triggerMap.GoalCreators.Add(goal);
            Triggers.Add(triggerMap);
        }
    }

    private bool TriggerCondition()
    {
        var isRaining = WeatherComponent.Instance.CurrentWeather == WeatherType.Rain;
        var isNotCovered = !Agent.State.BeliefComponent.GetBelief(Facts.Predicates.IsCovered).Evaluate();
        return isRaining && isNotCovered;
    }
}

public class IsCoveredCondition : ISatisfactionCondition
{
    public float GetSatisfaction(IState state)
    {
        if (WeatherComponent.Instance.CurrentWeather == WeatherType.Normal)
        {
            return 1.0f;
        }

        if (state.BeliefComponent.GetBelief(Facts.Predicates.IsCovered).Evaluate())
        {
            return 1.0f;
        }
        return 0.0f;
    }
}