using System;
using Godot;
using UGOAP.BehaviourSystem.Actions;

public partial class SitDownActionLogic : Node, IActionLogic
{
    public event Action LogicFinished;

    public void Start() { }

    public void Stop() { }

    public void Update(float delta)
    {
        LogicFinished?.Invoke();
    }
}
