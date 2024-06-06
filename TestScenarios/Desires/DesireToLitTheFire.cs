using Godot;
using UGOAP.BehaviourSystem.Desires;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.CommonUtils.FastName;
using UGOAP.SmartObjects;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using System.Collections.Generic;
using System;

namespace UGOAP;

[GlobalClass]
public partial class DesireToLitTheFire : Desire
{
    private FirePit _firePit;

    protected override void ConfigureTriggers()
    {
        SmartObjectBlackboard.Instance.TryGetGenericObject<FirePit>(out _firePit);
        if (_firePit == null)
        {
            SmartObjectBlackboard.Instance.ObjectRegistered += OnSmartObjectRegistered;
            return;
        }
        CreateTriggerMapping();
    }

    private void OnSmartObjectRegistered(ISmartObject @object)
    {
        if (@object is FirePit firePit)
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
        _agent.State.BeliefComponent.AddBelief(fireLitBelief);
        var trigger = new Belief.BeliefBuilder(Facts.Predicates.FireIsNotLit)
            .WithCondition(() => _firePit.IsLit == false)
            .Build();
        var goal = () => new Goal.Builder(Facts.Goals.LitFire)
            .WithSatisfactionCondition(new FireLitCondition())
            .WithPriority(10.0f)
            .WithDesiredEffect(new Belief.BeliefBuilder(Facts.Predicates.FireIsLit).WithCondition(() => true).Build())
            .Build();
        triggerMapping.Add(trigger, new List<Func<Goal>>() { goal });
    }
}
