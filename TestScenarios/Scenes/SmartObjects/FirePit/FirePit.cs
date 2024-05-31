using System;
using System.Collections.Generic;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.SmartObjects;

namespace UGOAP;

[GlobalClass]
public partial class FirePit : Node2D, ISmartObject
{
    public FastName Id { get; private set; }
    public Vector2 Location => GlobalPosition;
    public HashSet<IActionBuilder> SuppliedActionBuilders { get; } = new();
    public bool IsLit { get; private set; } = true;

    private Timer timer;
    private AnimatedSprite2D _animatedSprite;

    public override void _Ready()
    {
        Id = new FastName(Name);
        SuppliedActionBuilders.Add(new LightFireActionBuilder(this));

        _animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _animatedSprite.Play("Lit");

        timer = new Timer() { OneShot = true, WaitTime = 10.0f };
        timer.Timeout += UnLitFire;
        AddChild(timer);
        timer.Start();

        SmartObjectBlackboard.Instance.RegisterObject(Id, this);
    }

    private void UnLitFire()
    {
        IsLit = false;
        _animatedSprite.Play("UnLit");
    }

    public void LightFire()
    {
        IsLit = true;
        _animatedSprite.Play("Lit");
        timer.Start();
    }
}
