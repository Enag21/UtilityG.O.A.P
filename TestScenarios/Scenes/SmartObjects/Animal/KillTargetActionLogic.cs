using System;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.TestScenarios.Components;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Animal;

public partial class KillTargetActionLogic : IActionLogic
{
    public event Action LogicFinished;
    private readonly IDamagable _target;
    private readonly Components.HealthComponent _targetHealthComponent;
    private readonly Components.HurtBoxComponent _damageBoxComponent;

    public KillTargetActionLogic(IDamagable damagable, Components.HealthComponent healthComponent, Components.HurtBoxComponent damageBoxComponent)
    {
        _target = damagable;
        _targetHealthComponent = healthComponent;
        _damageBoxComponent = damageBoxComponent;
    }

    public void Start()
    {
        _targetHealthComponent.HealthDepleted += () => LogicFinished?.Invoke();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void Update(float delta)
    {
        _target.Damage(_damageBoxComponent.Attack);
    }
}