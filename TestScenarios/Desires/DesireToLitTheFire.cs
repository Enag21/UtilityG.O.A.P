using Godot;
using UGOAP.BehaviourSystem.Desires;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;
using UGOAP.TestScenarios.Goals;

namespace UGOAP.TestScenarios.Desires;

[GlobalClass]
public partial class DesireToLitTheFire : Desire
{
    private Scenes.SmartObjects.FirePit.FirePit _firePit;

    protected override void ConfigureTriggers()
    {
        SmartObjectBlackboard.Instance.TryGetGenericObject<Scenes.SmartObjects.FirePit.FirePit>(out _firePit);
        if (_firePit == null)
        {
            SmartObjectBlackboard.Instance.ObjectRegistered += OnSmartObjectRegistered;
            return;
        }
        CreateTriggerMapping();
    }

    private void OnSmartObjectRegistered(ISmartObject @object)
    {
        if (@object is Scenes.SmartObjects.FirePit.FirePit firePit)
        {
            _firePit = firePit;
            CreateTriggerMapping();
            SmartObjectBlackboard.Instance.ObjectRegistered -= OnSmartObjectRegistered;
        }
    }

    private void CreateTriggerMapping()
    {
        var fireLitBelief = new Belief.BeliefBuilder(Facts.Predicates.FireIsLit)
            .WithCondition(() => _firePit.IsLit == true)
            .Build();
        Agent.State.BeliefComponent.AddBelief(fireLitBelief);
        var goal = () => new Goal.Builder(Facts.Goals.LitFire)
            .WithSatisfactionCondition(new FireLitCondition())
            .WithPriority(10.0f)
            .WithDesiredEffect(new BeliefEffect(Facts.Predicates.FireIsLit, () => true))
            .Build();
        var trigger = new Trigger.Builder()
            .WithCondition(() => _firePit.IsLit == false)
            .WithGoalCreator(goal)
            .Build();
        Triggers.Add(trigger);

    }
}
