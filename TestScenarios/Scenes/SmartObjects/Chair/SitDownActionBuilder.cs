using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
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
        return new BasicAction.Builder(new FastName("SitDown"), agent, _provider)
            .WithInRangeRequired()
            .WithActionLogic(new SitDownActionLogic())
            .WithCost(() => agent.Location.DistanceTo(_provider.Location))
            .WithStateEffect(
                new Belief.BeliefBuilder(Facts.Predicates.IsSitting)
                    .WithCondition(() => true)
                    .Build()
            )
            .Build();
    }
}