using System.Collections.Generic;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.SmartObjects;
using UGOAP.TestScenarios.Components;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Animal;

[GlobalClass]
public partial class SmartAnimal : CharacterBody2D, ISmartObject, IDamagable
{
    [Export] private Components.HealthComponent _healthComponent;
    [Export] private Components.HitBoxComponent _hitBoxComponent;

    public FastName Id { get; private set; }
    public Vector2 Location => GlobalPosition;
    public HashSet<IActionBuilder> SuppliedActionBuilders { get; private set; } = new HashSet<IActionBuilder>();

    public override void _Ready()
    {
        Id = new FastName(Name);
        SmartObjectBlackboard.Instance.RegisterObject(Id, this);
        _healthComponent.HealthDepleted += OnHealthDepleted;
    }

    private void OnHealthDepleted()
    {
        SmartObjectBlackboard.Instance.UnregisterObject(Id);
        QueueFree();
    }

    public void Damage(Scripts.Attack attack)
    {
        _hitBoxComponent.Damage(attack);
    }
}
