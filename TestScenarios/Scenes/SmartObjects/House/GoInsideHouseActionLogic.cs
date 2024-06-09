using System;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.House;

public partial class GoInsideHouseActionLogic : IActionLogic
{
    public event Action LogicFinished;
    private readonly SmartHouse _smartHouse;
    private readonly IAgent _agent;

    public GoInsideHouseActionLogic(ISmartObject smartHouse, IAgent agent) => (_smartHouse, _agent) = (smartHouse as SmartHouse, agent);

    public void Start()
    {
        if (_agent.State.BeliefComponent.GetBelief(new FastName($"At {_smartHouse.Id}")).Evaluate())
        {
            return;
        }
        _agent.NavigationComponent.SetDestination(_smartHouse.Location);
    }

    public void Stop()
    {
        _agent.NavigationComponent.SetDestination(_agent.Location);
    }

    public void Update(float delta)
    {
        if (_agent.State.BeliefComponent.GetBelief(new FastName($"At {_smartHouse.Id}")).Evaluate())
        {
            LogicFinished?.Invoke();
        }
    }
}