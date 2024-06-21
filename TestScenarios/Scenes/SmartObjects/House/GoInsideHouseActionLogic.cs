using System;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.SmartObjects;
using UGOAP.TestScenarios.Systems.WeatherSystem;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.House;

public partial class GoInsideHouseActionLogic : IActionLogic
{
    public event Action LogicFinished;
    public event Action LogicFailed;
    private readonly SmartHouse _smartHouse;
    private readonly IAgent _agent;

    public GoInsideHouseActionLogic(ISmartObject smartHouse, IAgent agent) => (_smartHouse, _agent) = (smartHouse as SmartHouse, agent);

    public void Start() { }

    public void Stop() { }

    public void Update(float delta)
    {
        var isNotRaining = WeatherComponent.Instance.CurrentWeather == WeatherType.Normal;
        if (isNotRaining)
        {
            LogicFailed?.Invoke();
        }
        else if (_agent.Location.DistanceTo(_smartHouse.Location) < 0.3f)
        {
            LogicFinished?.Invoke();
        }
    }
}