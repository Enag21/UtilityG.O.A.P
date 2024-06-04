using System;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public partial class MovementAction : ActionBase
{
    public override event Action ActionFinished;
    private readonly IAgent _agent;
    private readonly ISmartObject _smartObject;

    public MovementAction(FastName actionName, IAgent agent, ISmartObject provider)
        : base(actionName, agent, provider)
    {
        _agent = agent;
        _smartObject = provider;
        ActionLogic = new MoveActionLogic(agent.NavigationComponent, provider.Location);
        ActionLogic.LogicFinished += () =>
        {
            ApplyEffects(agent.State);
            ActionFinished?.Invoke();
        };

        Effects.Add(
            new Belief.BeliefBuilder(new FastName($"At {provider.Id}"))
                .WithCondition(() => true)
                .Build()
        );

        var beliefFactory = new BeliefFactory(_agent);
        _agent.State.BeliefComponent.UpdateBelief(
            beliefFactory.CreateLocationBelief(
                new FastName($"At {_smartObject.Id}"),
                _smartObject.Location,
                0.1f
            )
        );

    }

    protected override void ApplyEffects(IState state) { }

    public override void _EnterTree()
    {
        AddChild(ActionLogic as Node);
    }
}
