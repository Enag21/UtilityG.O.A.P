using System.Collections.Generic;
using System.Linq;
using Godot;
using UGOAP.Agent;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.SmartObjects;

namespace UGOAP.AgentComponents;

[GlobalClass]
public partial class ActionManagerComponent : Node, IActionManager
{
    public HashSet<IAction> AvailableActions { get; } = new HashSet<IAction>();
    private IAgent _agent;
    private IBeliefComponent _beliefComponent;

    public override void _Ready()
    {
        _agent = GetOwner<IAgent>();
        _beliefComponent = _agent.State.BeliefComponent;
        SmartObjectBlackboard.Instance.ObjectRegistered += OnSmartObjectRegistered;
        SmartObjectBlackboard.Instance.ObjectDeregistered += OnSmartObjectDeregistered;
    }

    private void OnSmartObjectDeregistered(ISmartObject @object)
    {
        var actions = AvailableActions.Where(a => a.Provider == @object);
        RemoveLocationBelief(@object);
        foreach (var action in actions)
        {
            AvailableActions.Remove(action);
            if (action is Node node) node.QueueFree();
        }
    }

    private void OnSmartObjectRegistered(ISmartObject @object)
    {
        AvailableActions.UnionWith(CreateSmartObjectActions(@object));
        AddLocationBelief(@object);
    }

    private void SetUpActions()
    {
        foreach (var kvp in SmartObjectBlackboard.Instance.Objects)
        {
            var actions = CreateSmartObjectActions(kvp.Value);
            AddLocationBelief(kvp.Value);
            AvailableActions.UnionWith(actions);
        }
    }

    private HashSet<IAction> CreateSmartObjectActions(ISmartObject smartObject)
    {
        var actions = new HashSet<IAction>();
        foreach (var action in smartObject.SuppliedActionBuilders.Select(actionBuilder => actionBuilder.Build(_agent)))
        {
            actions.Add(action);
            AddChild(action as Node);
        }
        return actions;
    }

    private void AddLocationBelief(ISmartObject smartObject)
    {
        _beliefComponent.AddBelief(new Belief.BeliefBuilder(new FastName($"At {smartObject.Id}"))
            .WithCondition(() => _agent.Location.DistanceTo(smartObject.Location) < 0.2f)
            .Build());
    }

    private void RemoveLocationBelief(ISmartObject smartObject)
    {
        _beliefComponent.RemoveBelief(new FastName($"At {smartObject.Id}"));
    }
}