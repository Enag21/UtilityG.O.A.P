using Godot;

namespace UGOAP.TestScenarios.Components;

[GlobalClass]
public partial class HitBoxComponent : Area2D, IDamagable
{
    [Signal] public delegate void DamagedEventHandler(Scripts.Attack attack);

    public void Damage(Scripts.Attack attack)
    {
        EmitSignal(SignalName.Damaged, attack);
    }
}