using Godot;
using UGOAP.BehaviourSystem.Desires;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Goals.SatisfactionConditions;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.TestScenarios.Desires;

[GlobalClass]
public partial class DesireToSitDown : Desire
{
    protected override void ConfigureTriggers()
    {
        var goal = () => new Goal.Builder(new FastName("SitDown"))
            .WithSatisfactionCondition(new SitDownCondition())
            .WithPriority(1.0f)
            .WithDesiredEffect(new BeliefEffect(Facts.Predicates.IsSitting, () => true))
            .Build();
        var trigger = new Trigger.Builder()
            .WithCondition(() => Agent.State.BeliefComponent.GetBelief(Facts.Predicates.IsSitting).Evaluate() == false)
            .WithCondition(() => Agent.GoalDriver.ActiveGoals.Count == 0)
            .WithGoalCreator(goal)
            .Build();
        Triggers.Add(trigger);
    }
}

internal class SitDownCondition : ISatisfactionCondition
{
    public float GetSatisfaction(IState state)
    {
        var isSitting = state.BeliefComponent.GetBelief(Facts.Predicates.IsSitting).Evaluate();
        if (isSitting)
        {
            return 1.0f;
        }
        return 0.0f;
    }
}
