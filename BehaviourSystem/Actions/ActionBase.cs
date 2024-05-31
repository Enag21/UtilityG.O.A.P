using System;
using System.Collections.Generic;
using Godot;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
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
    public List<IParameterModifier> ParameterModifiers { get; protected set; } = new List<IParameterModifier>();
    public Effects Effects { get; protected set; } = new Effects();
    public Preconditions Preconditions { get; protected set; } = new Preconditions();
    protected Func<float> CostFunction = () => 1.0f;
    protected IActionLogic ActionLogic;
    protected readonly IState State;

    protected ActionBase(FastName actionName, IState state, ISmartObject provider)
    {
        State = state;
        ActionName = actionName;
        Provider = provider;
    }

    protected ActionBase()
    {
    }

    protected virtual void ApplyEffects(IState state)
    {
        Effects.ApplyEffects(state);
        ParameterModifiers.ForEach(modifier => modifier.Modify(state.ParameterManager.GetParameter(modifier.ParameterName)));
    }

    public virtual void Start() => ActionLogic.Start();
    public virtual void Stop() => ActionLogic.Stop();
    public virtual void Update(float delta) => ActionLogic.Update(delta);
}