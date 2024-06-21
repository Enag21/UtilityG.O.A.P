using System;
using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;
using UGOAP.TestScenarios.Components;
using UGOAP.TestScenarios.Scripts;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Animal;

[GlobalClass]
public partial class SmartAnimal : CharacterBody2D, ISmartObject, IDamagable, ISensable, IEntity
{
    public event Action<IDamagable> Killed;
    [Export] public float Health { get; set; } = 25.0f;
    public FastName Id { get; private set; }
    public Vector2 Location => GlobalPosition;
    public HashSet<IActionBuilder> SuppliedActionBuilders { get; private set; } = new HashSet<IActionBuilder>();
    public Dictionary<FastName, float> Data { get; private set; } = new Dictionary<FastName, float>();

    public override void _Ready()
    {
        Id = new FastName(Name);
        SmartObjectBlackboard.Instance.RegisterObject(Id, this);
        Data[Names.Health] = Health;
    }

    private void OnHealthDepleted(IEntity killer)
    {
        if (killer is IAgent agent)
        {
            agent.State.BeliefComponent.UpdateBelief(new Belief.BeliefBuilder(Facts.Predicates.HasFood).WithCondition(() => true).Build());
        }
        SmartObjectBlackboard.Instance.UnregisterObject(Id);
        QueueFree();
    }

    public void Damage(Attack attack)
    {
        Health -= attack.Damage;
        Data[Names.Health] = Health;
        if (Health <= 0.0f)
        {
            Killed?.Invoke(this);
            OnHealthDepleted(attack.Owner);
        }
    }
}
