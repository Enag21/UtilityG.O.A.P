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
    [Export] public float Weight { get; private set; } = 1.0f;
    public FastName DesireName { get; private set; }
    public List<Goal> Goals { get; private set; } = new List<Goal>();
    protected  HashSet<Belief> triggers = new HashSet<Belief>();
    protected IAgent _agent;
    private List<Goal> _markedForRemoval = new List<Goal>();

    public override void _Ready()
    {
        DesireName = new FastName(Name);
        _agent = GetOwner<IAgent>();
        OnReady();
    }

    public void Update()
    {
        EvaluateTriggers();
        RemoveSatisfiedGoals();
        Goals.ForEach(g => g.Update(_agent.State));
    }

    protected virtual void EvaluateTriggers()
    {
        foreach (var trigger in triggers)
        {
            if (!trigger.Evaluate()) continue;
            var goal = CreateGoal(trigger.Predicate);
            if (Goals.Any(g => g.Name == goal.Name)) continue;
            goal.GoalSatisfied += () => { _markedForRemoval.Add(goal); };
            Goals.Add(goal);
        }
    }

    public float ComputeSatisfaction(IState state = null)
    {
        IState stateToUse = state ?? _agent.State;
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

    protected abstract void OnReady();
    protected abstract Goal CreateGoal(FastName triggerName);
}