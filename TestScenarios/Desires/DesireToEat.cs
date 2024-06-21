using System.Linq;
using Godot;
using UGOAP.BehaviourSystem.Desires;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Goals.SatisfactionConditions;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.TestScenarios.Components;

namespace UGOAP.TestScenarios.Desires;

[GlobalClass]
public partial class DesireToEat : Desire
{
    protected override void ConfigureTriggers()
    {
        Triggers.Add(HasFoodTrigger());
        Triggers.Add(NeedsToHuntTrigger());
    }

    private Trigger HasFoodTrigger()
    {
        var goal = () => new Goal.Builder(new FastName("Eat"))
            .WithSatisfactionCondition(new NoHungryCondition())
            .Build();
        var trigger = new Trigger.Builder()
            .WithCondition(() => Agent.State.BeliefComponent.GetBelief(Facts.Predicates.IsHungry).Evaluate())
            .WithCondition(() => Agent.State.BeliefComponent.GetBelief(Facts.Predicates.HasFood).Evaluate())
            .WithGoalCreator(goal)
            .Build();
        return trigger;
    }

    private Trigger NeedsToHuntTrigger()
    {
        var trigger = new Trigger.Builder()
            .WithCondition(() => Agent.State.BeliefComponent.GetBelief(Facts.Predicates.IsHungry).Evaluate())
            .WithCondition(() => Agent.State.BeliefComponent.GetBelief(Facts.Predicates.HasFood).Evaluate() == false)
            .WithGoalCreator(() => HuntClosestTarget())
            .Build();
        return trigger;
    }

    private Goal HuntClosestTarget()
    {
        var targets = Agent.State.BeliefComponent.GetSpecificEntityBeliefs<IDamagable>();
        var closestTarget = targets.OrderBy(x => x.EntityFluent.Entity.Location.DistanceTo(Agent.Location)).FirstOrDefault().EntityFluent.Entity;

        return new Goal.Builder(new FastName("Hunt"))
            .WithSatisfactionCondition(new HuntTargetCondition(closestTarget as IDamagable))
            .WithEffect(new FluentEffect.Builder(new EntityFluent.AboutEntity(closestTarget).WithHealth(closestTarget.Data[Names.Health]).Create()).Build())
            .WithPriority(1.0f)
            .Build();
    }
}

public class HuntTargetCondition : ISatisfactionCondition
{
    private readonly IDamagable _target;

    public HuntTargetCondition(IDamagable target) => _target = target;

    public float GetSatisfaction(IState state)
    {
        var entity = state.BeliefComponent.GetBeliefAboutEntity(_target as IEntity)?.EntityFluent;
        if (entity == null)
        {
            return 0.0f;
        }
        var health = entity.Data[Names.Health];

        return 1.0f - health / 100.0f;
    }
}

public class NoHungryCondition : ISatisfactionCondition
{
    public float GetSatisfaction(IState state)
    {
        if (state.BeliefComponent.GetBelief(Facts.Predicates.IsHungry).Evaluate())
        {
            return 0.0f;
        }
        return 1.0f;
    }
}