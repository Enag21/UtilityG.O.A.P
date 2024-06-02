using System;
using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP;

[GlobalClass]
public partial class SenseComponent : Area2D, ISensor
{
    private IAgent _agent;
    private List<ISensable> _sensables = new List<ISensable>();
    private List<ISensable> _markedForRemoval = new List<ISensable>();
    private Timer _updateTimer = new Timer() { OneShot = false, WaitTime = 1.0f, Autostart = true };

    public override void _Ready()
    {
        _agent = GetOwner<IAgent>();
        AddChild(_updateTimer);
        _updateTimer.Timeout += () => {
            RemoveMarkedForRemovalSensables();
            foreach (var sensable in _sensables)
            {
                UpdateBeliefs(sensable);
            }
        };
        AreaEntered += OnAreaEntered;
        AreaExited += OnAreaExited;
    }

    public void UpdateBeliefs(ISensable sensable)
    {
        foreach (var (_, belief) in sensable.BeliefComponent.Beliefs)
        {
            _agent.State.BeliefComponent.UpdateBelief(belief);
        }
    }

    public void OnAreaEntered(Area2D area)
    {
        if (area is ISensable sensable)
        {
            _sensables.Add(sensable);
        }
    }

    private void OnAreaExited(Area2D area)
    {
        if (area is ISensable sensable)
        {
            _markedForRemoval.Add(sensable);
        }
    }

    private void RemoveMarkedForRemovalSensables()
    {
        if (_markedForRemoval.Count == 0) return;
        foreach (var sensable in _markedForRemoval)
        {
            _sensables.Remove(sensable);
        }
        _markedForRemoval.Clear();
    }
}
