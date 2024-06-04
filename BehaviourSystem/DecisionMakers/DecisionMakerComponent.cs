using System.Collections.Generic;
using System.Linq;
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
    private List<Plan> _planList = new List<Plan>();
    private List<Plan> _markedForSimulation = new List<Plan>();

    public override void _Ready()
    {
        _utilityRater = new DesireUtilityRater(DesireComponent.Desires);
        _state = GetOwner<IAgent>().State;
    }

    public Plan Decide(HashSet<Plan> plans)
    {
        _planList.Clear();
        _planList = plans.ToList();
        SimulatePlans();
        RatePlans();
        _planList.OrderByDescending(plan => plan.Utility);
        return _planList.FirstOrDefault();
    }

    public void ReComputeUtilityForPlan(Plan plan)
    {
        var newUtility = _utilityRater.RateUtility(plan.State);
        plan.SetUtility(newUtility);
    }

    private void SimulatePlans()
    {
        foreach(var plan in _planList)
        {
            if (plan.State is not null) { continue; }
            _markedForSimulation.Add(plan);
        }

        if (_markedForSimulation.Count == 0) { return; }

        foreach(var plan in _markedForSimulation)
        {
            _planList.Remove(plan);
            var planSimulatedState = new PlanSimulator(_state, plan).Simulate();
            _planList.Add(plan with { State = planSimulatedState });
        }
        _markedForSimulation.Clear();
    }

    private void RatePlans()
    {
        foreach (var plan in _planList)
        {
            var utility = _utilityRater.RateUtility(plan.State);
            plan.SetUtility(utility);
        }
    }
}