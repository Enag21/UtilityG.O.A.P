using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.AgentComponents;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.DecisionMakers;

[GlobalClass]
public partial class DecisionMakerComponent : Node, IDecisionMaker
{
    [Export]
    public DesireComponent DesireComponent { get; set; }
    private IUtilityRater _utilityRater;
    private IState _state;
    private readonly Dictionary<Plan, IState> _simulatedStates = new Dictionary<Plan, IState>();
    private readonly PriorityQueue<Plan, float> _ratedPlans = new PriorityQueue<Plan, float>();
    public override void _Ready()
    {
        _utilityRater = new DesireUtilityRater(DesireComponent.Desires);
        _state = GetOwner<IAgent>().State;
    }

    public Plan Decide(HashSet<Plan> plans)
    {
        _simulatedStates.Clear();
        _ratedPlans.Clear();
        SimulatePlans(plans);
        RatePlans();
        return _ratedPlans.Dequeue();
    }

    private void SimulatePlans(HashSet<Plan> plans)
    {
        foreach (var plan in plans)
        {
            if (plan.State is not null)
            {
                _simulatedStates.Add(plan, plan.State);
                continue;
            }
            var planSimulator = new PlanSimulator(_state, plan);
            _simulatedStates.Add(plan, planSimulator.Simulate());
        }
    }

    private void RatePlans()
    {
        foreach (var (plan, state) in _simulatedStates)
        {
            var utility = _utilityRater.RateUtility(state);
            _ratedPlans.Enqueue(plan, 1.0f / utility);
        }
    }
}