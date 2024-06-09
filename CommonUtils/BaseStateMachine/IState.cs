using System;

namespace UGOAP.CommonUtils.BaseStateMachine;

public interface IState
{
    event Action<StateTransition> StateChanged;
    FastName.FastName StateName { get; }
    void Enter();
    void Exit();
    void Update(float delta);
    void PhysicsUpdate(float delta);
}

public record StateTransition(IState From, FastName.FastName To);