using System;
using System.Collections.Generic;
using Godot;
using UGOAP.BehaviourSystem.Goals;

namespace UGOAP.BehaviourSystem.Desires;

public class Trigger
{
    public event Action<Trigger> Triggered = delegate { };
    public List<Func<bool>> Conditions { get; } = new List<Func<bool>>();
    public List<Func<Goal>> GoalCreators { get; } = new List<Func<Goal>>();
    private List<Goal> _activeGoals = new List<Goal>();

    public void Evaluate()
    {
        if (_activeGoals.Count > 0) return;
        foreach (var condition in Conditions)
        {
            if (!condition())
            {
                return;
            }
        }
        Triggered(this);
        return;
    }

    public List<Goal> CreatedGoals()
    {
        foreach (var goalCreator in GoalCreators)
        {
            var goal = goalCreator();
            GD.Print($"Created Goal: {goal.Name}");
            _activeGoals.Add(goal);
            goal.GoalSatisfied += () => _activeGoals.Remove(goal);
        }
        return _activeGoals;
    }

    public class Builder
    {
        private readonly Trigger _trigger;

        public Builder() => _trigger = new Trigger();

        public Builder WithCondition(Func<bool> condition)
        {
            _trigger.Conditions.Add(condition);
            return this;
        }

        public Builder WithGoalCreator(Func<Goal> goalCreator)
        {
            _trigger.GoalCreators.Add(goalCreator);
            return this;
        }

        public Trigger Build() => _trigger;
    }
}