using System.Collections.Generic;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.SmartObjects;
using UGOAP.TestScenarion.Scenes.SmartObjects.Tree.Actions.ChopTree;

namespace UGOAP.TestScenarion.Scenes.SmartObjects.Tree;

[GlobalClass]
public partial class TreeSmartObject : Node2D, ISmartObject
{
    public FastName Id { get; private set; }
    public Vector2 Location => GlobalPosition;
    public HashSet<IActionBuilder> SuppliedActionBuilders { get; private set; } = new HashSet<IActionBuilder>();

    public override void _Ready()
    {
        Id = new FastName(Name);
        SuppliedActionBuilders.Add(new ChopTreeActionBuilder(this));
        SmartObjectBlackboard.Instance.RegisterObject(Id, this);
    }

    public void ChopTree()
    {
        SmartObjectBlackboard.Instance.UnregisterObject(Id);
        QueueFree();
    }
}