using System;
using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public abstract partial class ActionBase : Node, IAction
{
    public abstract event Action ActionFinished;
    public CommonUtils.FastName.FastName ActionName { get; protected set; }
    public ISmartObject Provider { get; protected set; }
    public float Cost => CostFunction();
    public List<IParameterModifier> ParameterModifiers { get; protected set; } =
        new List<IParameterModifier>();
    public Effects Effects { get; protected set; } = new Effects();
    public Preconditions Preconditions { get; protected set; } = new Preconditions();
    protected Func<float> CostFunction = () => 1.0f;
    protected IActionLogic ActionLogic;
    protected readonly IAgent Agent;
    protected bool RequiresInRange = false;
    private bool _inRange = false;

    protected ActionBase(CommonUtils.FastName.FastName actionName, IAgent state, ISmartObject provider)
    {
        this.Agent = state;
        ActionName = actionName;
        Provider = provider;
    }

    protected virtual void ApplyEffects(IState state)
    {
        Effects.ApplyEffects(state);
        ParameterModifiers.ForEach(modifier =>
            modifier.Modify(state.ParameterManager.GetParameter(modifier.ParameterType))
        );
    }

    public virtual void Start()
    {
        if (RequiresInRange && !Agent.State.BeliefComponent.GetBelief(new CommonUtils.FastName.FastName($"At {Provider.Id}")).Evaluate())
        {
            _inRange = false;
            Agent.NavigationComponent.SetDestination(Provider.Location);
            Agent.NavigationComponent.NavigationFinished += () => { ActionLogic.Start(); _inRange = true; };
            return;
        }
        ActionLogic.Start();
    }

    public virtual void Stop() => ActionLogic.Stop();

    public virtual void Update(float delta)
    {
        if (RequiresInRange && !_inRange) return;
        ActionLogic.Update(delta);
    }
}