using System;
using Godot;
using UGOAP.AgentComponents.Interfaces;

namespace UGOAP.AgentComponents;

[GlobalClass]
public partial class MoveComponent : Node, INavigationComponent
{
    [Export] public CharacterBody2D Actor { get; private set; }
    [Export] public float MaxSpeed { get; private set; } = 100.0f;

    public event Action NavigationFinished;

    private Vector2 _destination = Vector2.Zero;
    private float _range = 1.0f;

    public override void _Process(double delta)
    {
        if (_destination == Vector2.Zero) return;
        if (Actor.Position.DistanceTo(_destination) < _range)
        {
            _destination = Vector2.Zero;
            NavigationFinished?.Invoke();
            return;
        }

        Actor.Position = Actor.Position.MoveToward(_destination, (float)(delta * MaxSpeed));
    }

    public void SetDestination(Vector2 destination, float range)
    {
        _destination = destination;
        _range = range;
    }
}