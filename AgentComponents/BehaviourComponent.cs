using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.DecisionMakers;
using UGOAP.BehaviourSystem.Planners;

namespace UGOAP.AgentComponents;

[GlobalClass]
public partial class BehaviourComponent : Node
{
    [Export] public DesireComponent DesireComponentNode { get; private set; }
    private IGoalDriver GoalDriver => DesireComponentNode as IGoalDriver;

    [Export] public ActionManagerComponent ActionManagerComponentNode { get; private set; }
    private IActionManager ActionManagerComponent => ActionManagerComponentNode as IActionManager;

    [Export] public ActionExecutionComponent PlanExecutionerComponentNode { get; private set; }
    private IPlanExecutioner PlanExecutionComponent => PlanExecutionerComponentNode as IPlanExecutioner;

    [Export] public DecisionMakerComponent DecisionMakerComponentNode { get; private set; }
    private IDecisionMaker DecisionMakerComponent => DecisionMakerComponentNode as IDecisionMaker;

    private PlannerComponent _plannerComponent;
    private bool _behaviourRunning = false;
    private Plan _currentPlan;
    private IAgent _agent;

    public override void _Ready()
    {
        _agent = GetOwner<IAgent>();
        PlanExecutionComponent.PlanFinished += () => _behaviourRunning = false;
        _plannerComponent = new PlannerComponent(new AStarPlanner(new BeliefsHeuristic()));
    }

    public override void _Process(double delta)
    {
        if (!_behaviourRunning && AnyUnfulfilledGoals())
        {
            GD.Print("SelectANewPlan");
            SelectANewPlan();
        }
    }

    public void RequestReplanning()
    {
        var newPlan = ComputePlan();
        // We need to recompute the utility of the current plan because now there might be new desires or goals
        DecisionMakerComponent.ReComputeUtilityForPlan(_currentPlan);
        if (newPlan.Utility > _currentPlan.Utility)
        {
            PlanExecutionComponent.StopCurrentPlan();
            _currentPlan = newPlan;
            PlanExecutionComponent.LoadPlan(_currentPlan);
            _behaviourRunning = true;
        }
    }

    private bool AnyUnfulfilledGoals() => GoalDriver.ActiveGoals.Count > 0;

    private void SelectANewPlan()
    {
        _currentPlan = ComputePlan();
        PlanExecutionComponent.LoadPlan(_currentPlan);
        _behaviourRunning = true;
    }

    private Plan ComputePlan()
    {
        var plans = new HashSet<Plan>();
        GoalDriver.ActiveGoals.ForEach(goal => plans.Add(_plannerComponent.Plan(ActionManagerComponent.AvailableActions, goal, _agent.State)));
        return DecisionMakerComponent.Decide(plans);
    }
}