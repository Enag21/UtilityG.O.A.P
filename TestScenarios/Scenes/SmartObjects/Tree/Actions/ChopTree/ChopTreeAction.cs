using System;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarion.Scenes.SmartObjects.Tree.Actions.ChopTree;

public partial class ChopTreeActionLogic : Node, IActionLogic
{
    public event Action LogicFinished;

    private TreeSmartObject _smartTree;
    public ChopTreeActionLogic(ISmartObject smartObject) => _smartTree = (TreeSmartObject)smartObject;
    public void Start()
    {
        _smartTree.ChopTree();
        LogicFinished?.Invoke();
    }
}