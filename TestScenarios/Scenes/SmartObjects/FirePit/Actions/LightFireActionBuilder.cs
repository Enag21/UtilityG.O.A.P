using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.FirePit.Actions;

public partial class LightFireActionBuilder : Node, IActionBuilder
{
    private readonly ISmartObject _smartObject;
    public LightFireActionBuilder(ISmartObject smartObject) => _smartObject = smartObject;

    public IAction Build(IAgent agent)
    {
        var action = new ActionBuilder<InRangeAction>(new FastName("LightFire"), new LightFireActionLogic(_smartObject, agent), _smartObject, agent)
            .WithCost(() => agent.Location.DistanceTo(_smartObject.Location))
            .WithEffect(new BeliefEffect(Facts.Predicates.FireIsLit, () => true))
            .WithPrecondition(new Belief.BeliefBuilder(Facts.Preconditions.HasWood).WithCondition(() => true).Build())
            .BuildAction();
        return action;
    }
}
