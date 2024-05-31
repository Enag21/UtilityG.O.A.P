using Godot;
using UGOAP.CommonUtils.FastName;
using UGOAP.SmartObjects;

namespace UGOAP.KnowledgeRepresentation.BeliefSystem;

public class BeliefFactory
{
    readonly ISmartObject _smartObject;

    public BeliefFactory(ISmartObject smartObject) => _smartObject = smartObject;

    public Belief CreateLocationBelief(FastName predicate, Vector2 targetLocation, float range)
    {
        return new Belief.BeliefBuilder(predicate)
            .WithCondition(() => InRange(_smartObject.Location, targetLocation, range))
            .WithLocation(() => targetLocation)
            .Build();
    }

    private bool InRange(Vector2 location, Vector2 target, float range) =>
        location.DistanceTo(target) <= range;
}