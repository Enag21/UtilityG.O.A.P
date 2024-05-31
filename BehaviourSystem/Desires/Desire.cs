using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Goals;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;

namespace UGOAP.BehaviourSystem.Desires;

public abstract partial class Desire : Node
{
    [Export] public float Weight { get; private set; } = 1.0f;
    public FastName DesireName { get; private set; }
    public List<Goal> Goals { get; private set; } = new List<Goal>();
    protected  HashSet<Belief> triggers = new HashSet<Belief>();
    private IAgent _agent;

    public override void _Ready()
    {
        DesireName = new FastName(Name);
        _agent = GetOwner<IAgent>();
        OnReady();
    }

    public void Update()
    {
        EvaluateTriggers();
        Goals.ForEach(g => g.Update(_agent.State));
    }

    protected virtual void EvaluateTriggers()
    {
        foreach (var trigger in triggers)
        {
            if (!trigger.Evaluate()) continue;
            var goal = CreateGoal(trigger.Predicate);
            if (Goals.Contains(goal)) continue;
            goal.GoalSatisfied += () => { Goals.Remove(goal); };
            Goals.Add(goal);
        }
    }

    public float ComputeSatisfaction()
    {
        if (Goals.Count == 0)
        {
            return 1.0f;
        }

        var valueSum = 0.0f;
        var weightSum = 0.0f;
        foreach (var goal in Goals)
        {
            var satisfaction = goal.GetSatisfaction(_agent.State);
            valueSum += satisfaction * goal.Priority;
            weightSum += goal.Priority;
        }

        return valueSum / weightSum;
    }

    protected abstract void OnReady();
    protected abstract Goal CreateGoal(FastName triggerName);
}