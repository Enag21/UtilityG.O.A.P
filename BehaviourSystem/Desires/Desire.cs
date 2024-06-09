using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Desires;

public abstract partial class Desire : Node
{
    [Export] public Curve WeightCurve { get; private set; }
    [Export] public float Weight { get; private set; } = 1.0f;
    public event Action<Desire> DesireTriggered = delegate { };
    public FastName DesireName { get; private set; }
    public List<Goal> Goals { get; private set; } = new List<Goal>();
    protected List<TriggerGoalMap> Triggers = new List<TriggerGoalMap>();
    protected IAgent Agent;
    private List<Goal> _markedForRemoval = new List<Goal>();

    protected class TriggerGoalMap
    {
        public Belief Trigger { get; set; }
        public List<Func<Goal>> GoalCreators { get; set; } = new List<Func<Goal>>();
        public bool IsActive => _activeGoals.Count > 0;
        private List<Goal> _activeGoals = new List<Goal>();

        public TriggerGoalMap(Belief trigger)
        {
            Trigger = trigger;
        }

        public List<Goal> CreatedGoals()
        {
            foreach (var goal in GoalCreators)
            {
                var goalCreated = goal();
                goalCreated.GoalSatisfied += () => { _activeGoals.Remove(goalCreated); };
                _activeGoals.Add(goalCreated);
            }
            return _activeGoals;
        }
    }

    public override void _Ready()
    {
        DesireName = new FastName(Name);
        Agent = GetOwner<IAgent>();
        ConfigureTriggers();
    }

    public void Update()
    {
        EvaluateTriggers();
        RemoveSatisfiedGoals();
        Goals.ForEach(g => g.Update(Agent.State));
    }

    protected virtual void EvaluateTriggers()
    {
        var triggersToProcess = Triggers.Where(trigger => !trigger.IsActive).Where(trigger => trigger.Trigger.Evaluate()).ToList();
        foreach (var trigger in triggersToProcess)
        {
            var goals = trigger.CreatedGoals();
            goals.ForEach(goal => goal.GoalSatisfied += () => { _markedForRemoval.Add(goal); });
            Goals.AddRange(goals);
        }
        if (triggersToProcess.Count > 0)
        {
            DesireTriggered(this);
        }
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