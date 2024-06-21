using System;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.TestScenarios.Components;
using UGOAP.TestScenarios.Scripts;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Weapon;

public partial class AttackActionLogic : Node, IActionLogic
{
    public event Action LogicFinished;
    public event Action LogicFailed;

    private readonly IDamagable _target;
    private readonly Attack _attack;
    private readonly IAgent _user;
    private bool _canAttack = true;
    public AttackActionLogic(IDamagable damagable, Attack attack, IAgent user) => (_target, _attack, _user) = (damagable, attack, user);

    public override void _Ready()
    {
    }

    public void Start()
    {
        _canAttack = true;
    }
    public void Stop()
    {
    }

    public void Update(float delta)
    {
        if (_canAttack && InRange())
        {
            _target.Damage(_attack);
            _canAttack = false;
            LogicFinished?.Invoke();
        }
        else if (!InRange())
        {
            _user.NavigationComponent.SetDestination(_target.Location, 0.3f);
        }
    }

    private bool InRange() => _target.Location.DistanceTo(_user.Location) < 0.3f;
}
