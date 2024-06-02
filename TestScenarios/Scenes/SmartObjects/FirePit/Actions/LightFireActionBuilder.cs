using System;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;

namespace UGOAP;

public partial class LightFireActionBuilder : Node, IActionBuilder
{
    private readonly ISmartObject _smartObject;
    public LightFireActionBuilder(ISmartObject smartObject) => _smartObject = smartObject;

    public IAction Build(IAgent agent)
    {
        var action = new BasicAction.Builder(new FastName("LightFire"), agent.State, _smartObject)
            .WithCost(() => agent.Location.DistanceTo(_smartObject.Location))
            .WithActionLogic(new LightFireActionLogic(_smartObject, agent))
            .WithStateEffect(new Belief.BeliefBuilder(Facts.Predicates.FireIsLit).WithCondition(() => true).Build())
            .WithStatePrecondition(new Belief.BeliefBuilder(new FastName($"At {_smartObject.Id}")).WithCondition(() => true).Build())
            .WithStatePrecondition(new Belief.BeliefBuilder(Facts.Preconditions.HasWood).WithCondition(() => true).Build())
            .Build();
        return action;
    }
}
