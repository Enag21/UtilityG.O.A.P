using System;
using Godot;
using UGOAP.CommonUtils.BaseStateMachine;
using UGOAP.CommonUtils.FastName;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Animal;

[GlobalClass]
public partial class AnimalIdleState : BaseState
{
    public override event Action<StateTransition> StateChanged;
    public override FastName StateName => AnimalStates.Idle;
    private Timer _idleTimer;

    public override void Enter()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();

        _idleTimer = new Timer()
        {
            WaitTime = rng.RandfRange(3.0f, 5.0f),
            Autostart = true,
        };
        _idleTimer.Timeout += () =>
        {
            StateChanged?.Invoke(new StateTransition(this, AnimalStates.Wander));
        };
        AddChild(_idleTimer);
    }

    public override void Exit()
    {
        _idleTimer.Stop();
        _idleTimer.QueueFree();
        _idleTimer = null;
    }

    public override void Update(float delta) { }
    public override void PhysicsUpdate(float delta) { }
}