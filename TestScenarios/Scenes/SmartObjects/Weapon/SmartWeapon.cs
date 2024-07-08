using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.AgentComponents;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;
using UGOAP.TestScenarios.Components;
using UGOAP.TestScenarios.Scripts;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Weapon;

[GlobalClass]
public partial class SmartWeapon : Area2D, ISmartObject
{
    [Export] public BaseAgent2D Agent { get; private set; }
    [Export] ActionManagerComponent ActionManager;
    [Export] Attack Attack;
    public FastName Id { get; private set; }
    public Vector2 Location => GlobalPosition;
    public HashSet<IActionBuilder> SuppliedActionBuilders { get; private set; } = new HashSet<IActionBuilder>();
    private Dictionary<IDamagable, IAction> _targets = new Dictionary<IDamagable, IAction>();

    public override void _Ready()
    {
        Id = new FastName(Name);
        Attack.Owner = Agent;
    }

    public void RegisterTarget(IDamagable target)
    {
        if (!_targets.ContainsKey(target))
        {
            var action = new ActionBuilder<BasicAction>(new FastName($"Attack {target.Id}"), new AttackActionLogic(target, Attack, Agent), this, Agent)
                .WithEffect(DamageTargetEffect(target))
                .BuildAction();
            ActionManager.RegisterAction(action);
            _targets.Add(target, action);
            target.Killed += OnTargetDied;
        }
    }

    public void OnTargetDied(IDamagable target)
    {
        ActionManager.RemoveAction(_targets[target]);
        _targets.Remove(target);
    }

    private IEffect DamageTargetEffect(IDamagable target)
    {
        var entity = Agent.State.BeliefComponent.GetBeliefAboutEntity(target as IEntity)?.EntityFluent.Entity;
        if (entity == null)
        {
            return null;
        }

        var effect = new FluentEffect.Builder(new EntityFluent.AboutEntity(entity).Create())
            .WithHealthModifier(- Attack.Damage)
            .WithConditionalEffect(Facts.Predicates.HasFood, () => true, (health) => health <= 0.0f)
            .Build();
        return effect;
    }
}