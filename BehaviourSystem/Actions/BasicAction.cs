using System;
using Godot;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public partial class BasicAction : ActionBase
{
    public override event Action ActionFinished;

    private BasicAction(FastName actionName, IState state, ISmartObject provider)
        : base(actionName, state, provider) { }

    public BasicAction()
    {
    }

    public override void _EnterTree()
    {
        AddChild(ActionLogic as Node);
    }

    public class Builder
    {
        readonly BasicAction _action;

        public Builder(FastName actionName, IState state, ISmartObject provider)
        {
            _action = new BasicAction(actionName, state, provider);
        }

        public Builder WithCost(Func<float> cost)
        {
            _action.CostFunction = cost;
            return this;
        }

        public Builder WithStateEffect(Belief stateEffect)
        {
            _action.Effects.Add(stateEffect);
            return this;
        }

        public Builder WithStatePrecondition(Belief statePrecondition)
        {
            _action.Preconditions.Add(statePrecondition);
            return this;
        }

        public Builder WithModifier(IParameterModifier modifier)
        {
            _action.ParameterModifiers.Add(modifier);
            return this;
        }

        public Builder WithActionLogic(IActionLogic actionLogic)
        {
            _action.ActionLogic = actionLogic;
            _action.ActionLogic.LogicFinished += () =>
            {
                _action.ApplyEffects(_action.State);
                _action.ActionFinished?.Invoke();
            };
            return this;
        }

        public IAction Build() => _action;
    }
}