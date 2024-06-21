using System;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.SmartObjects;

namespace UGOAP.TestScenarios.Scenes.SmartObjects.Tree.Actions.ChopTree;

public partial class ChopTreeActionLogic : Node, IActionLogic
{
    public event Action LogicFinished;
    public event Action LogicFailed;


    private TreeSmartObject _smartTree;
    private bool _treeChopped = false;
    public ChopTreeActionLogic(ISmartObject smartObject) => _smartTree = (TreeSmartObject)smartObject;

    public void Update(float delta)
    {
        if (!_treeChopped)
        {
            _treeChopped = true;
            _smartTree.ChopTree();
            LogicFinished?.Invoke();
        }
    }

    public void Start() { }
    public void Stop() => _treeChopped = false;
}