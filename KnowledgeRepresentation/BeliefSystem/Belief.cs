using System;
using System.Collections.Generic;
using Godot;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.SmartObjects;

namespace UGOAP.KnowledgeRepresentation.BeliefSystem;

public class Belief : ICopyable<Belief>
{
    public FastName Predicate { get; set; }
    public EntityFluent EntityFluent { get; set; } = null;
    private Func<bool> _condition = () => false;
    private Func<Vector2> _location = () => Vector2.Zero;

    Belief(FastName predicate) => Predicate = predicate;

    public bool Evaluate() => _condition();

    public Belief Copy()
    {
        return new BeliefBuilder(Predicate)
            .WithCondition(_condition.Clone() as Func<bool>)
            .WithLocation(_location.Clone() as Func<Vector2>)
            .WithEntity(EntityFluent?.Copy())
            .Build();
    }

    internal Belief Invert()
    {
        var result = Evaluate();
        _condition = () => !result;
        return this;
    }

    public class BeliefBuilder
    {
        readonly Belief _belief;

        public BeliefBuilder(FastName predicate) => _belief = new Belief(predicate);

        public BeliefBuilder WithCondition(Func<bool> condition)
        {
            _belief._condition = condition;
            return this;
        }

        public BeliefBuilder WithLocation(Func<Vector2> location)
        {
            _belief._location = location;
            return this;
        }

        public BeliefBuilder WithEntity(EntityFluent entity)
        {
            _belief.EntityFluent = entity;
            return this;
        }
        public Belief Build() => _belief;
    }
}

public record Entity(ISmartObject SmartObject, Func<float> Health = null);

public interface IEntity
{
    FastName Id { get; }
    Vector2 Location { get; }
    Dictionary<FastName, float> Data { get; }
}

public class EntityFluent : ICopyable<EntityFluent>
{
    public IEntity Entity { get; set; } = null;
    public Dictionary<FastName, float> Data { get; set; } = new();

    EntityFluent(IEntity entity) => Entity = entity;

    public class AboutEntity
    {
        private readonly EntityFluent _entity;

        public AboutEntity(IEntity entity) => _entity = new EntityFluent(entity);

        public AboutEntity WithHealth(float health)
        {
            _entity.Data[new FastName("Health")] = health;
            return this;
        }

        public AboutEntity WithHunger(float hunger)
        {
            _entity.Data[new FastName("Hunger")] = hunger;
            return this;
        }

        public EntityFluent Create() => _entity;
    }

    public EntityFluent Copy()
    {
        var newdata = new Dictionary<FastName, float>(Data);
        var fluent = new EntityFluent(Entity) { Data = newdata };
        return fluent;
    }
}