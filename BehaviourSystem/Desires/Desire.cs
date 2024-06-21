using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Desires;

public abstract partial class Desire : Node
{
    [Export] public Curve WeightCurve { get; private set; }
    [Export] public float Weight { get; private set; } = 1.0f;
    [Export] public ParameterType ParameterWeight { get; private set; }
    public event Action<Desire> DesireTriggered = delegate { };
    public FastName DesireName { get; private set; }
    public List<Goal> Goals { get; private set; } = new List<Goal>();
    protected List<Trigger> Triggers = new List<Trigger>();
    protected IAgent Agent;
    private List<Goal> _markedForRemoval = new List<Goal>();

    public override void _Ready()
    {
        DesireName = new FastName(Name);
        Agent = GetOwner<IAgent>();
        ConfigureTriggers();
        Triggers.ForEach(trigger => trigger.Triggered += OnTriggerTriggered);
    }

    private void OnTriggerTriggered(Trigger trigger)
    {
        var newGoals = trigger.CreatedGoals();
        newGoals.ForEach(goal => goal.GoalSatisfied += () => { _markedForRemoval.Add(goal); });
        Goals.AddRange(newGoals);
        DesireTriggered(this);
    }

    public void Update()
    {
        Triggers.ForEach(trigger => trigger.Evaluate());
        RemoveSatisfiedGoals();
        Goals.ForEach(g => g.Update(Agent.State));
    }

    public float ComputeSatisfaction(IState state = null)
    {
        IState stateToUse = state ?? Agent.State;
        if (Goals.Count == 0)
        {
            return 1.0f;
        }

        var valueSum = 0.0f;
        var weightSum = 0.0f;
        foreach (var goal in Goals)
        {
            var satisfaction = goal.GetSatisfaction(stateToUse);
            valueSum += satisfaction * goal.Priority;
            weightSum += goal.Priority;
        }

        return valueSum / weightSum;
    }

    private void RemoveSatisfiedGoals()
    {
        if (_markedForRemoval.Count == 0) return;
        foreach (var goal in _markedForRemoval)
        {
            Goals.Remove(goal);
        }
        _markedForRemoval.Clear();
    }

    protected abstract void ConfigureTriggers();
}
