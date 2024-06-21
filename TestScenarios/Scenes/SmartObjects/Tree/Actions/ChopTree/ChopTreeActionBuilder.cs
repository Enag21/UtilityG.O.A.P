using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Planners;
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
        var action = new ActionBuilder<InRangeAction>(new FastName("ChopTree"), new ChopTreeActionLogic(_smartObject), _smartObject, agent)
            .WithCost(() => agent.Location.DistanceTo(_smartObject.Location))
            .WithEffect(new BeliefEffect(Facts.Effects.HasWood, () => true))
            .BuildAction();
        return action;
    }
}