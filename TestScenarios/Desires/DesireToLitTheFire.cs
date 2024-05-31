using System;
using Godot;
using UGOAP.BehaviourSystem.Desires;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.CommonUtils.FastName;
using UGOAP.SmartObjects;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;

namespace UGOAP;

[GlobalClass]
public partial class DesireToLitTheFire : Desire
{
    private FirePit _firePit;

    protected override Goal CreateGoal(FastName triggerName)
    {
        var goal = new Goal.Builder(Facts.Goals.LitFire)
            .WithSatisfactionCondition(new FireLitCondition(_firePit))
            .WithPriority(10.0f)
            .WithDesiredEffect(Facts.Effects.FireLit)
            .Build();
        return goal;
    }

    protected override void OnReady()
    {
        SmartObjectBlackboard.Instance.ObjectRegistered += OnSmartObjectRegistered;
    }

    private void OnSmartObjectRegistered(ISmartObject @object)
    {
        if (@object is FirePit firePit)
        {
            _firePit = firePit;
            triggers.Add(new Belief.BeliefBuilder(Facts.Predicates.FireIsUnlit)
                .WithCondition(() => _firePit.IsLit == false)
                .Build());
            SmartObjectBlackboard.Instance.ObjectRegistered -= OnSmartObjectRegistered;
        }
    }
}
