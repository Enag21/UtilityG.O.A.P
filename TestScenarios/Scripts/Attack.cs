using Godot;

namespace UGOAP.TestScenarios.Scripts;

public partial class Attack : Node
{
    public float Damage { get; set; }

    public Attack(float damage)
    {
        Damage = damage;
    }
}