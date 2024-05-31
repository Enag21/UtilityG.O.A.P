using System;
using Godot;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Planners;

namespace UGOAP.AgentComponents;

[GlobalClass]
public partial class ActionExecutionComponent : Node, IPlanExecutioner
{
    public event Action PlanFinished;
    private IAction CurrentAction { get; set; }
    private Plan _currentPlan;

    public override void _Process(double delta)
    {
        CurrentAction?.Update((float)delta);
    }

    public void LoadPlan(Plan plan)
    {
        _currentPlan = plan;
    }

    private void LoadAction()
    {
        if (_currentPlan == null || _currentPlan.Actions.Count == 0)
        {
            return;
        }
        CurrentAction = _currentPlan.Actions.Dequeue();
        CurrentAction.ActionFinished += OnActionFinished;
    }

    private void OnActionFinished()
    {
        CurrentAction.ActionFinished -= OnActionFinished;
        CurrentAction.Stop();
        CurrentAction = null;

        if (_currentPlan.Actions.Count != 0) return;
        GD.Print("Plan Finished");
        _currentPlan = null;
        PlanFinished?.Invoke();
    }
}