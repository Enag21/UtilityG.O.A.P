using System;
using Godot;
using UGOAP.CommonUtils.BaseStateMachine;
using UGOAP.CommonUtils.FastName;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Animal;

[GlobalClass]
public partial class AnimalWanderState : BaseState
{
    public override FastName StateName => AnimalStates.Wander;
    public override event Action<StateTransition> StateChanged;

    [Export] private float _minWanderTime = 5.0f;
    [Export] private float _maxWanderTime = 10.0f;
    [Export] private float _wanderSpeed = 50.0f;

    private SmartAnimal _smartAnimal;
    private Vector2 _wanderDirection = Vector2.Zero;
    private Timer _wanderTimer;

    public override void _Ready()
    {
        _smartAnimal = GetParent<StateMachine>().Actor as SmartAnimal;
    }

    public override void Enter()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        var randomRotation = rng.RandfRange(0.0f, 360.0f);
        _wanderDirection = Vector2.Up.Rotated(Mathf.DegToRad(randomRotation));

        _wanderTimer = new Timer()
        {
            WaitTime = rng.RandfRange(_minWanderTime, _maxWanderTime),
            Autostart = true,
        };
        _wanderTimer.Timeout += () =>
        {
            StateChanged?.Invoke(new StateTransition(this, AnimalStates.Idle));
        };
        AddChild(_wanderTimer);
    }

    public override void Exit()
    {
        _wanderTimer.Stop();
        _wanderTimer.QueueFree();
        _wanderTimer = null;
    }
    public override void Update(float delta) { }
    public override void PhysicsUpdate(float delta)
    {
        _smartAnimal.Velocity = _wanderDirection.Normalized() * _wanderSpeed;
        _smartAnimal.MoveAndSlide();
    }
}