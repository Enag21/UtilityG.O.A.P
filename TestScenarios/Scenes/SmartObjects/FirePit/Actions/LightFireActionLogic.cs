using System;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.SmartObjects;

namespace UGOAP;

public partial class LightFireActionLogic : Node, IActionLogic
{
    public event Action LogicFinished;
    FirePit _firePit;

    public LightFireActionLogic(ISmartObject firePit) => _firePit = firePit as FirePit; 

    public void Start()
    {
        _firePit.LightFire();
        LogicFinished?.Invoke();
    }
}
