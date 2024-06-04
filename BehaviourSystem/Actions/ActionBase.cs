using System;
using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public abstract partial class ActionBase : Node, IAction
{
    public abstract event Action ActionFinished;
    public FastName ActionName { get; protected set; }
    public ISmartObject Provider { get; protected set; }
    public float Cost => CostFunction();
    public List<IParameterModifier> ParameterModifiers { get; protected set; } =
        new List<IParameterModifier>();
    public Effects Effects { get; protected set; } = new Effects();
    public Preconditions Preconditions { get; protected set; } = new Preconditions();
    protected Func<float> CostFunction = () => 1.0f;
    protected IActionLogic ActionLogic;
    protected readonly IAgent agent;
    protected bool RequiresInRange = false;
    private bool _inRange = false;

    protected ActionBase(FastName actionName, IAgent state, ISmartObject provider)
    {
        this.agent = state;
        ActionName = actionName;
        Provider = provider;
    }

    protected virtual void ApplyEffects(IState state)
    {
        Effects.ApplyEffects(state);
        ParameterModifiers.ForEach(modifier =>
            modifier.Modify(state.ParameterManager.GetParameter(modifier.ParameterName))
        );
    }

    public virtual void Start()
    {
        if (RequiresInRange && !agent.State.BeliefComponent.GetBelief(new FastName($"At {Provider.Id}")).Evaluate())
        {
            _inRange = false;
            agent.NavigationComponent.SetDestination(Provider.Location);
            agent.NavigationComponent.NavigationFinished += () => { ActionLogic.Start(); _inRange = true; };
            return;
        }
        ActionLogic.Start();
    }

    public virtual void Stop() => ActionLogic.Stop();

    public virtual void Update(float delta)
    {
        if (!_inRange) return;
        ActionLogic.Update(delta);
    }
}