using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
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
        var action = new BasicAction.Builder(new FastName("GoInsideHouse"), agent, _smartHouse)
            .WithCost(() => agent.Location.DistanceTo(_smartHouse.Location))
            .WithActionLogic(new GoInsideHouseActionLogic(_smartHouse, agent))
            .WithStateEffect(new Belief.BeliefBuilder(Facts.Predicates.IsCovered).WithCondition(() => true).Build())
            .Build();
        return action;
    }
}