using System.Collections.Generic;
using Godot;
using UGOAP.AgentComponents;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.Actions;
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

        _hungerTimer = new Timer();
        AddChild(_hungerTimer);
        _hungerTimer.WaitTime = 3.0f;
        _hungerTimer.Timeout += OnHungerTimerTimeout;
        _hungerTimer.Start();
    }

    private void OnHungerTimerTimeout()
    {
        State.ParameterManager.UpdateParameter(ParameterType.Hunger, -25.0f);
        GD.Print($"Hunger: {State.ParameterManager.GetParameter(ParameterType.Hunger).Value}");
    }
}
