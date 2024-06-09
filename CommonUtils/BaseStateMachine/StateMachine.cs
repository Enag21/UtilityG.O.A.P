using System.Collections.Generic;
using Godot;


namespace UGOAP.CommonUtils.BaseStateMachine;

[GlobalClass]
public partial class StateMachine : Node
{
    [Export] public Node2D Actor { get; private set; }
    [Export] private BaseState _initialState;
    public IState CurrentState { get; private set; }

    private Dictionary<FastName.FastName, IState> _states = new();


    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is IState state)
            {
                _states.Add(state.StateName, state);
                state.StateChanged += OnStateChanged;
            }
        }
        CurrentState = _initialState;
        CurrentState.Enter();
    }

    public override void _Process(double delta) => CurrentState?.Update((float)delta);
    public override void _PhysicsProcess(double delta) => CurrentState?.PhysicsUpdate((float)delta);

    private void OnStateChanged(StateTransition transition)
    {
        if (transition.From != CurrentState) return;

        var nextState = _states.GetValueOrDefault(transition.To);
        if (nextState == null) return;

        if (CurrentState != null)
        {
            CurrentState.Exit();
        }
        CurrentState = nextState;
        CurrentState.Enter();
    }
}