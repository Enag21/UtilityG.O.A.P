using System;
using System.Linq;
using Godot;
using UGOAP.Blackboard;
using UGOAP.CommonUtils;

namespace UGOAP.SmartObjects;

public partial class SmartObjectBlackboard : Blackboard<ISmartObject>
{
    public override void _Ready()
    {
        //RegisterAllSmartObjects();
    }

    private void RegisterAllSmartObjects()
    {
        var smartObjects = GetTree().GetNodesInGroup("SmartObject");
        foreach (var node in smartObjects)
        {
            node.Ready += () => RegisterSmartObject(node);
        }
    }

    private void RegisterSmartObject(Node node)
    {
        if (node is ISmartObject smartObject)
        {
            RegisterObject(smartObject.Id, smartObject);
        }
    }
}