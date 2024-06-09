using Godot;
using UGOAP.BehaviourSystem.Desires;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Goals.SatisfactionConditions;
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
        var trigger = new Belief.BeliefBuilder(new FastName("NotSitting"))
            .WithCondition(() => Agent.State.BeliefComponent.GetBelief(Facts.Predicates.IsSitting).Evaluate() == false).Build();
        var goal = () => new Goal.Builder(new FastName("SitDown"))
            .WithSatisfactionCondition(new SitDownCondition())
            .WithPriority(1.0f)
            .WithDesiredEffect(new Belief.BeliefBuilder(Facts.Predicates.IsSitting).WithCondition(() => true).Build())
            .Build();
        var triggerMap = new TriggerGoalMap(trigger);
        triggerMap.GoalCreators.Add(goal);
        Triggers.Add(triggerMap);
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