using System;
using Godot;
using UGOAP.CommonUtils.FastName;

namespace UGOAP.TestScenarios.Components;

public interface IDamagable
{
    event Action<IDamagable> Killed;
    FastName Id { get; }
    float Health { get; set; }
    Vector2 Location { get; }
    void Damage(Scripts.Attack attack);
}