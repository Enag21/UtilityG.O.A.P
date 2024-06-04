using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;

[GlobalClass]
public partial class Chair : Area2D, ISmartObject
{
    public FastName Id { get; private set; }

    public Vector2 Location => GlobalPosition;

    public HashSet<IActionBuilder> SuppliedActionBuilders { get; private set; } = new();

    public override void _Ready()
    {
        Id = new FastName(Name);
        SuppliedActionBuilders.Add(new SitDownActionBuilder(this));
        SmartObjectBlackboard.Instance.RegisterObject(Id, this);
        BodyExited += OnBodyExited;
    }

    private void OnBodyExited(Node2D body)
    {
        if (body is IAgent agent)
        {
            agent.State.BeliefComponent.UpdateBelief(new Belief.BeliefBuilder(Facts.Predicates.IsSitting).WithCondition(() => false).Build());
        }
    }
}
