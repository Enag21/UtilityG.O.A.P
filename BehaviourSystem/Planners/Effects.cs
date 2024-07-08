using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using UGOAP.CommonUtils.ExtensionMethods;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Planners;

public interface IEffect
{
    void ApplyEffect(IState state);
    bool FulfillsAnyRequiredEffects(IState state);
}

public class BeliefEffect : IEffect
{
    public Belief Effect { get; private set; }
    public BeliefEffect(FastName effectName, Func<bool> effect)
    {
        Effect = new Belief.BeliefBuilder(effectName).WithCondition(effect).Build();
    }

    public virtual void ApplyEffect(IState state) => state.BeliefComponent.UpdateBelief(Effect);

    public virtual bool FulfillsAnyRequiredEffects(IState state)
    {
        var unfuldilledEffects = state.BeliefComponent.Beliefs.Where(b => !b.Value.Evaluate());
        foreach (var (predicate, belief) in unfuldilledEffects)
        {
            if (Effect.Predicate == predicate)
            {
                return true;
            }
        }
        return false;
    }
}

public class FluentEffect : IEffect
{
    public EntityFluent EntityFluent { get; private set; }
    public Dictionary<FastName, float> DataModifiers { get; set; } = new Dictionary<FastName, float>();
    private ConditionalEffect _conditionalEffect;
    private FluentEffect() {}

    public void ApplyEffect(IState state)
    {
        var belief = state.BeliefComponent.GetBeliefAboutEntity(EntityFluent.Entity);
        if (belief == null) return;
        foreach (var (data, modifier) in DataModifiers)
        {
            var currentValue = belief.EntityFluent.Data[data];
            var newValue = Mathf.Clamp(currentValue + modifier, 0.0f, 100.0f);
            belief.EntityFluent.Data[data] = newValue;

            if (_conditionalEffect != null)
            {
                if (_conditionalEffect.Condition(newValue))
                {
                    state.BeliefComponent.UpdateBelief(_conditionalEffect.Effect);
                }
            }
        }
    }

    public bool FulfillsAnyRequiredEffects(IState state)
    {
        if (_conditionalEffect == null) return false;
        var unfuldilledEffects = state.BeliefComponent.Beliefs.Where(b => !b.Value.Evaluate());
        foreach (var (predicate, belief) in unfuldilledEffects)
        {
            if (_conditionalEffect.Effect.Predicate == predicate)
            {
                return true;
            }
        }
        return false;
    }

    public class Builder
    {
        private FluentEffect _fluentEffect;
        private ConditionalEffect _conditionalEffect;
        public Builder(EntityFluent entityFluent)
        {
            _fluentEffect = new FluentEffect() { EntityFluent = entityFluent };
        }

        public Builder WithConditionalEffect(FastName effectName, Func<bool> effect, Func<float, bool> condition)
        {
            _conditionalEffect = new ConditionalEffect(effectName, effect, condition);
            _fluentEffect._conditionalEffect = _conditionalEffect;
            return this;
        }

        public Builder WithHealthModifier(float modifier)
        {
            _fluentEffect.DataModifiers.Add(Names.Health, modifier);
            return this;
        }

        public Builder WithHungerModifier(float modifier)
        {
            _fluentEffect.DataModifiers.Add(Names.Hunger, modifier);
            return this;
        }
        public FluentEffect Build() => _fluentEffect;
    }

    private class ConditionalEffect
    {
        public Belief Effect { get; }
        public Func<float, bool> Condition { get; }
        public ConditionalEffect(FastName effectName, Func<bool> effect, Func<float, bool> condition)
        {
            Effect = new Belief.BeliefBuilder(effectName).WithCondition(effect).Build();
            Condition = condition;
        }
    }
}