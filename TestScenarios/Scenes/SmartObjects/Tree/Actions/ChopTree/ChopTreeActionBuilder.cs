using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Tree.Actions.ChopTree;

public class ChopTreeActionBuilder : IActionBuilder
{
    private readonly ISmartObject _smartObject;

    public ChopTreeActionBuilder(ISmartObject smartObject) => _smartObject = smartObject;

    public IAction Build(IAgent agent)
    {
        var action = new BasicAction.Builder(new FastName("ChopTree"), agent, _smartObject)
            .WithInRangeRequired()
            .WithCost(() => agent.Location.DistanceTo(_smartObject.Location))
            .WithActionLogic(new ChopTreeActionLogic(_smartObject))
            .WithStateEffect(new Belief.BeliefBuilder(Facts.Effects.HasWood).WithCondition(() => true).Build())
            .Build();
        return action;
    }
}