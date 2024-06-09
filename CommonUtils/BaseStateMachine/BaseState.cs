using System;
using Godot;

namespace UGOAP.CommonUtils.BaseStateMachine;

[GlobalClass]
public abstract partial class BaseState : Node, IState
{
    public abstract event Action<StateTransition> StateChanged;
    public abstract FastName.FastName StateName { get; }
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update(float delta);
    public abstract void PhysicsUpdate(float delta);
}