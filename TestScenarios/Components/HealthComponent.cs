using Godot;

namespace UGOAP.TestScenarios.Components;

[GlobalClass]
public partial class HealthComponent : Node
{
    [Signal] public delegate void HealthDepletedEventHandler();
    [Export] private HitBoxComponent _hitBox;
    [Export] private float _maxHealth = 100.0f;
    private float _health;

    public override void _Ready()
    {
        _health = _maxHealth;
        _hitBox.Damaged += OnDamaged;
    }

    private void OnDamaged(Scripts.Attack attack)
    {
        _health -= attack.Damage;
        if (_health <= 0.0f)
        {
            QueueFree();
        }
    }
}