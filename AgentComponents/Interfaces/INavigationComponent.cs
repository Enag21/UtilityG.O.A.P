using System;
using Godot;

namespace UGOAP.AgentComponents.Interfaces;

public interface INavigationComponent
{
    event Action NavigationFinished;
    void SetDestination(Vector2 destination);
}