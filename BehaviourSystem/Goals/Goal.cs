using System;
using System.Collections.Generic;
using System.Linq;
using UGOAP.BehaviourSystem.Goals.SatisfactionConditions;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.Goals;

public class Goal
{
    public FastName Name { get; set; }
    public event Action GoalSatisfied;
    public float Priority { get; set; }
    public Effects DesiredEffects { get; private set; } = new Effects();
    private readonly List<ISatisfactionCondition> _satisfactionConditions = new List<ISatisfactionCondition>();

    private Goal(FastName name) => Name = name;

    public void Update(IState state)
    {
        if (GetSatisfaction(state) >= 0.95f)
        {
            GoalSatisfied?.Invoke();
        }
    }
    public float GetSatisfaction(IState state)
    {
        if (_satisfactionConditions.Count == 0)
        {
            return 1.0f;
        }

        var valueSum = _satisfactionConditions.Sum(condition => condition.GetSatisfaction(state));
        return valueSum / _satisfactionConditions.Count;
    }

    public class Builder
    {
        readonly Goal _goal;

        public Builder(FastName name) => _goal = new Goal(name);

        public Builder WithPriority(float priority)
        {
            _goal.Priority = priority;
            return this;
        }

        public Builder WithDesiredEffect(FastName effect)
        {
            _goal.DesiredEffects.Add(effect);
            return this;
        }

        public Builder WithEffect(Belief effect)
        {
            _goal.DesiredEffects.Add(effect);
            return this;
        }

        public Builder WithSatisfactionCondition(ISatisfactionCondition condition)
        {
            _goal._satisfactionConditions.Add(condition);
            return this;
        }

        public Goal Build() => _goal;
    }
}