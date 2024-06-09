using System;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.TestScenarios.Components;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Animal;

public partial class DamageActionLogic : IActionLogic
{
    public event Action LogicFinished;

    private readonly IDamagable _target;
    public DamageActionLogic(IDamagable damagable) => _target = damagable;

    public void Start()
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void Update(float delta)
    {
        throw new NotImplementedException();
    }
}
