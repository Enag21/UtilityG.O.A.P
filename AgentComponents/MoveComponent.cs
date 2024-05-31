using System;
using Godot;
using UGOAP.AgentComponents.Interfaces;

namespace UGOAP.AgentComponents;

[GlobalClass]
public partial class MoveComponent : Node, INavigationComponent
{
    [Export] public CharacterBody2D Actor { get; private set; }
    [Export] public float MaxSpeed { get; private set; }
    
    public event Action NavigationFinished;
    
    private Vector2 _destination = Vector2.Zero;

    public override void _Process(double delta)
    {
        if (_destination == Vector2.Zero) return;
        if (Actor.Position.DistanceTo(_destination) < 1)
        {
            _destination = Vector2.Zero; 
            NavigationFinished?.Invoke(); 
            return;
        } 
        
        Actor.Position = Actor.Position.MoveToward(_destination, (float)(delta * MaxSpeed));
    }

    public void SetDestination(Vector2 destination) => _destination = destination;
}