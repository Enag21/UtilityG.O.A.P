using System.Collections.Generic;
using System.Linq;
using Godot;
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

    public override void _Ready()
    {
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

    private bool AnyUnfulfilledGoals() => GoalDriver.ActiveGoals.Count > 0;

    private void SelectANewPlan()
    {
        var plans = new HashSet<Plan>();
        GoalDriver.ActiveGoals.ForEach(goal => plans.Add(_plannerComponent.Plan(ActionManagerComponent.AvailableActions, goal)));
        var plan = DecisionMakerComponent.Decide(plans);
        PlanExecutionComponent.LoadPlan(plan);

        _behaviourRunning = true;
    }
}