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
        foreach (var action in actions)
        {
            AvailableActions.Remove(action);
            if (action is Node node) node.QueueFree();
        }
    }

    private void OnSmartObjectRegistered(ISmartObject @object)
    {
        AvailableActions.UnionWith(CreateSmartObjectActions(@object));
        AvailableActions.Add(CreateMovementAction(@object));
    }

    private void SetUpActions()
    {
        foreach (var kvp in SmartObjectBlackboard.Instance.Objects)
        {
            var actions = CreateSmartObjectActions(kvp.Value);
            var movementAction = CreateMovementAction(kvp.Value);
            AvailableActions.Add(movementAction);
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

    private IAction CreateMovementAction(ISmartObject smartObject)
    {
        var beliefFactory = new BeliefFactory(_agent);
        _beliefComponent.AddBelief(beliefFactory.CreateLocationBelief(new FastName($"At {smartObject.Id}"), smartObject.Location, 1.0f));

        var movementAction = new BasicAction.Builder(new FastName($"Move to {smartObject.Id}"), _agent.State, smartObject)
            .WithActionLogic(new MoveActionLogic(_agent.NavigationComponent, smartObject.Location))
            .WithEffect(new FastName($"At {smartObject.Id}"))
            .Build();
        AddChild(movementAction as Node);

        return movementAction;
    }
}