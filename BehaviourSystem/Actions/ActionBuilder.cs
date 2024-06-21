using System;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public class ActionBuilder<TAction> where TAction : IAction, new()
{
    private TAction _action;
    private IActionState _actionState;
    public ActionBuilder(FastName actionName, IActionLogic actionLogic, ISmartObject provider, IAgent user)
    {
        _actionState = new ActionState() { ActionName = actionName, Provider = provider, User = user };
        _action = new TAction() { ActionLogic = actionLogic, ActionState = _actionState };
    }

    public ActionBuilder<TAction> WithCost(Func<float> cost)
    {
        _actionState.Cost = cost;
        return this;
    }
    public ActionBuilder<TAction> WithEffect(IEffect stateEffect)
    {
        _actionState.Effects.Add(stateEffect);
        return this;
    }
    public ActionBuilder<TAction> WithPrecondition(Belief statePrecondition)
    {
        _actionState.Preconditions.Add(statePrecondition);
        return this;
    }

    public TAction BuildAction() => _action;
}