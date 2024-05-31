using System;
using Godot;
using UGOAP.AgentComponents.Interfaces;

namespace UGOAP.BehaviourSystem.Actions;

public class MoveActionLogic : IActionLogic
{
    public event Action LogicFinished;
    private readonly INavigationComponent _navigationComponent;
    private readonly Vector2 _destination;

    public MoveActionLogic(INavigationComponent navigationComponent, Vector2 destination)
    {
        _navigationComponent = navigationComponent;
        _destination = destination;

        _navigationComponent.NavigationFinished += () => LogicFinished?.Invoke();
    }

    public void Start() => _navigationComponent.SetDestination(_destination);

    public void Stop()
    {
    }

    public void Update(float delta)
    {
    }
}