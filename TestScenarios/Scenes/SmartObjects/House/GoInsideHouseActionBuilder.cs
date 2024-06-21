using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.House;

public class GoInsideHouseActionBuilder : IActionBuilder
{
    private readonly SmartHouse _smartHouse;
    public GoInsideHouseActionBuilder(ISmartObject smartObject) => _smartHouse = smartObject as SmartHouse;

    public IAction Build(IAgent agent)
    {
        var action = new ActionBuilder<InRangeAction>(new FastName("GoInsideHouse"), new GoInsideHouseActionLogic(_smartHouse, agent) ,_smartHouse, agent)
            .WithCost(() => agent.Location.DistanceTo(_smartHouse.Location))
            .WithEffect(new BeliefEffect(Facts.Predicates.IsCovered, () => false))
            .BuildAction();
        return action;
    }
}