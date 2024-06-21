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
    public IUtilityRater UtilityRater { get; private set; }
    private IState _state;
    private List<Plan> _planList = new List<Plan>();
    private List<Plan> _markedForSimulation = new List<Plan>();

    public override void _Ready()
    {
        UtilityRater = new DesireUtilityRater(DesireComponent.Desires);
        _state = GetOwner<IAgent>().State;
    }

    public Plan Decide(HashSet<Plan> plans)
    {
        _planList.Clear();
        _planList = plans.ToList();
        SimulatePlans();
        RatePlans();
        var bestPlan = GetBestPlan();
        return bestPlan;
    }

    public void ReComputeUtilityForPlan(Plan plan)
    {
        var newUtility = UtilityRater.RateUtility(plan.State);
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
            var utility = UtilityRater.RateUtility(plan.State);
            plan.SetUtility(utility);
        }
    }

    private Plan GetBestPlan()
    {
        var maxUtility = _planList[0].Utility;
        var bestPlan = _planList[0];
        foreach (var plan in _planList)
        {
            PrintPlan(plan);
            if (plan.Utility > maxUtility)
            {
                maxUtility = plan.Utility;
                bestPlan = plan;
            }
        }
        return bestPlan;
    }

    private void PrintPlan(Plan plan)
    {
        GD.Print($"Plan \n -Actions: {plan.Actions.Count} \n");
        foreach (var action in plan.Actions)
        {
            GD.Print($" -{action.ActionState.ActionName} \n");
        }
        GD.Print($" -Utility: {plan.Utility}");

    }
}