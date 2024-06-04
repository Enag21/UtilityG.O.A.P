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
        if (_currentPlan == null)
        {
            return;
        }

        if (_currentPlan.Actions.Count == 0 && CurrentAction == null)
        {
            _currentPlan = null;
            PlanFinished?.Invoke();
            return;
        }

        if (CurrentAction == null)
        {
            LoadAction();
        }

        CurrentAction.Update((float)delta);
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
        GD.Print($"Action Loaded: {CurrentAction.ActionName.ToString()}");
        CurrentAction.ActionFinished += OnActionFinished;
        CurrentAction.Start();
    }

    private void OnActionFinished()
    {
        GD.Print($"Action Finished: {CurrentAction.ActionName.ToString()}");
        StopAction();
        LoadAction();
    }

    private void StopAction()
    {
        CurrentAction.ActionFinished -= OnActionFinished;
        CurrentAction.Stop();
        CurrentAction = null;
    }

    public void StopCurrentPlan()
    {
        StopAction();
        _currentPlan = null;
    }
}