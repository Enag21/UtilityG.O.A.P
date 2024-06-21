using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Chair;

public partial class SitDownActionBuilder : IActionBuilder
{
    private ISmartObject _provider;

    public SitDownActionBuilder(ISmartObject provider)
    {
        _provider = provider;
    }

    public IAction Build(IAgent agent)
    {
        return new ActionBuilder<InRangeAction>(new FastName("SitDown"), new SitDownActionLogic(_provider, agent), _provider, agent)
            .WithCost(() => agent.Location.DistanceTo(_provider.Location))
            .WithEffect(
                new BeliefEffect(Facts.Predicates.IsSitting, () => true)
            )
            .BuildAction();
    }
}