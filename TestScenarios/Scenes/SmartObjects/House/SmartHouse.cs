using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.House;

[GlobalClass]
public partial class SmartHouse : Area2D, ISmartObject
{
    public FastName Id { get; private set; }

    public Vector2 Location => GlobalPosition;

    public HashSet<IActionBuilder> SuppliedActionBuilders { get; } = new HashSet<IActionBuilder>();

    public override void _Ready()
    {
        Id = new FastName(Name);
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
        SuppliedActionBuilders.Add(new GoInsideHouseActionBuilder(this));
        SmartObjectBlackboard.Instance.RegisterObject(Id, this);
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is IAgent agent)
        {
            agent.State.BeliefComponent.UpdateBelief(new Belief.BeliefBuilder(Facts.Predicates.IsCovered).WithCondition(() => false).Build());
        }
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is IAgent agent)
        {
            agent.State.BeliefComponent.UpdateBelief(new Belief.BeliefBuilder(Facts.Predicates.IsCovered).WithCondition(() => true).Build());
        }
    }
}