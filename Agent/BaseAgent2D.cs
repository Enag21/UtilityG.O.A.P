using System.Collections.Generic;
using Godot;
using UGOAP.AgentComponents;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.Agent;

[GlobalClass]
public partial class BaseAgent2D : CharacterBody2D, IAgent, IEntity
{
    [Export] public MoveComponent MoveComponent { get; private set; }
    [Export] public BehaviourComponent BehaviourComponent { get; private set; }
    [Export] StateManager _stateManager;
    [Export] DesireComponent _desireComponent;
    [Export] ActionManagerComponent _actionManager;
    public FastName Id { get; private set; }
    public Vector2 Location => GlobalPosition;
    public HashSet<IActionBuilder> SuppliedActionBuilders { get; private set; } = new();
    public INavigationComponent NavigationComponent => MoveComponent;
    public IState State => _stateManager.State;
    public IGoalDriver GoalDriver => _desireComponent;

    public Dictionary<FastName, float> Data { get; private set; } = new();


    private Timer _hungerTimer;

    public override void _Ready()
    {
        Id = new FastName(Name);
        State.ParameterManager.AddParameter(new Parameter(ParameterType.Hunger, 100.0f));
        Data[Names.Hunger] = State.ParameterManager.GetParameter(ParameterType.Hunger).Value;
        State.BeliefComponent.AddBelief(new Belief.BeliefBuilder(Facts.Predicates.IsHungry)
            .WithCondition(() => State.ParameterManager.GetParameter(ParameterType.Hunger).Value <= 25).Build());
        State.BeliefComponent.AddBelief(new Belief.BeliefBuilder(Facts.Predicates.NotHungry)
            .WithCondition(() => State.BeliefComponent.GetBelief(Facts.Predicates.IsHungry).Evaluate() == false).Build());
        var agentBelief = new Belief.BeliefBuilder(new FastName($"Agent {Id}"))
                    .WithEntity(new EntityFluent.AboutEntity(this).WithHunger(State.ParameterManager.GetParameter(ParameterType.Hunger).Value).Create())
                    .Build();

        State.BeliefComponent.UpdateBelief(agentBelief);

        _hungerTimer = new Timer();
        AddChild(_hungerTimer);
        _hungerTimer.WaitTime = 3.0f;
        _hungerTimer.Timeout += OnHungerTimerTimeout;
        _hungerTimer.Start();
        SetUpEatAction();
    }

    private void OnHungerTimerTimeout()
    {
        State.ParameterManager.UpdateParameter(ParameterType.Hunger, -25.0f);
        var agentBelief =State.BeliefComponent.GetBeliefAboutEntity(this);
        agentBelief.EntityFluent.Data[Names.Hunger] = State.ParameterManager.GetParameter(ParameterType.Hunger).Value;
        GD.Print($"Hunger: {State.ParameterManager.GetParameter(ParameterType.Hunger).Value}");
    }

    private void SetUpEatAction()
    {
        var eatAction = new ActionBuilder<BasicAction>(new FastName("Eat"), new EatActionLogic(this), this, this)
            .WithPrecondition(new Belief.BeliefBuilder(Facts.Predicates.HasFood).WithCondition(() => true).Build())
            .WithEffect(EatEffect())
            .BuildAction();

        _actionManager.RegisterAction(eatAction);
    }

    private IEffect EatEffect()
    {
        var agentFluent = new EntityFluent.AboutEntity(this)
            .WithHunger(State.ParameterManager.GetParameter(ParameterType.Hunger).Value)
            .Create();

        var effect = new FluentEffect.Builder(agentFluent)
            .WithHungerModifier(100.0f)
            .WithConditionalEffect(Facts.Predicates.NotHungry, () => true, (hunger) => hunger >= 25)
            .Build();
        return effect;
    }
}
