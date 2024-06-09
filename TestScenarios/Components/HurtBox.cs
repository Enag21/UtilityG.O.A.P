using Godot;

namespace UGOAP.TestScenarios.Components;

[GlobalClass]
public partial class HurtBoxComponent : Area2D, IDamager
{
    public Scripts.Attack Attack { get; private set; }

    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    private void OnAreaEntered(Area2D area)
    {
        if (area is IDamagable damagable)
        {
            damagable.Damage(Attack);
        }
    }
}