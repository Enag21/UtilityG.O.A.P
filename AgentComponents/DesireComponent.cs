using System.Collections.Generic;
using Godot;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.BehaviourSystem.Desires;

namespace UGOAP.AgentComponents;

[GlobalClass]
public partial class DesireComponent : Node, IGoalDriver
{
    [Export] public BehaviourComponent BehaviourComponent { get; private set; }
    [Export] public float MinimumDesireWeightForReplanning { get; private set; } = 5.0f;
    public List<Desire> Desires { get; } = new();
    public List<Goal> ActiveGoals => GetActiveGoals();

    public override void _Ready()
    {
        foreach (var child in GetChildren())
        {
            if (child is Desire desire)
            {
                Desires.Add(desire);
                desire.DesireTriggered += OnDesireTriggered;
            }
        }
    }

    public override void _Process(double delta)
    {
        Desires.ForEach(desire => desire.Update());
    }

    private List<Goal> GetActiveGoals()
    {
        var activeGoals = new List<Goal>();
        foreach (var desire in Desires)
        {
            activeGoals.AddRange(desire.Goals);
        }
        return activeGoals;
    }

    private void OnDesireTriggered(Desire desire)
    {
        if (desire.Weight > MinimumDesireWeightForReplanning)
        {
            BehaviourComponent.RequestReplanning();
        }
    }
}