using System;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.TestScenarios.Components;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Animal;

public partial class KillTargetActionLogic : IActionLogic
{
    public event Action LogicFinished;
    public event Action LogicFailed;

    private readonly IDamagable _target;

    public KillTargetActionLogic(IDamagable damagable, HealthComponent healthComponent, HurtBoxComponent damageBoxComponent)
    {
        _target = damagable;
    }

    public void Start()
    {
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void Update(float delta)
    {
    }
}