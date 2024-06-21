using System;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Chair;

public partial class SitDownActionLogic : Node, IActionLogic
{
    public event Action LogicFinished;
    public event Action LogicFailed;
    private ISmartObject _smartObject;
    private IAgent _agent;

    public SitDownActionLogic(ISmartObject smartObject, IAgent agent) => (_smartObject, _agent) = (smartObject, agent);
    public void Start() { }

    public void Stop() { }

    public void Update(float delta)
    {
        if (InRange())
        {
            LogicFinished?.Invoke();
        }
    }

    private bool InRange() => _smartObject.Location.DistanceTo(_agent.Location) < 0.3f;
}