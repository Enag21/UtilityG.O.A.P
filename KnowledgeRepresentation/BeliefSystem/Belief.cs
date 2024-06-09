using System;
using Godot;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.SmartObjects;

namespace UGOAP.KnowledgeRepresentation.BeliefSystem;

public class Belief : ICopyable<Belief>
{
    public CommonUtils.FastName.FastName Predicate { get; set; }
    private Func<bool> _condition = () => false;
    private Func<Vector2> _location = () => Vector2.Zero;
    private Func<ISmartObject> _entity = () => null;

    Belief(CommonUtils.FastName.FastName predicate) => Predicate = predicate;

    public bool Evaluate() => _condition();

    public Belief Copy()
    {
        return new BeliefBuilder(Predicate)
            .WithCondition(_condition.Clone() as Func<bool>)
            .WithLocation(_location.Clone() as Func<Vector2>)
            .WithEntity(_entity)
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

        public BeliefBuilder(CommonUtils.FastName.FastName predicate) => _belief = new Belief(predicate);

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

        public BeliefBuilder WithEntity(Func<ISmartObject> entity)
        {
            _belief._entity = entity;
            return this;
        }

        public Belief Build() => _belief;
    }
}