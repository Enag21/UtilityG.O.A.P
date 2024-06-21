using System;
using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public abstract partial class ActionBase : Node, IAction
{
    public abstract IActionLogic ActionLogic { get; set; }
    public abstract IActionState ActionState { get; set; }
    public event Action ActionFinished;

    public override void _EnterTree()
    {
        AddChild(ActionLogic as Node);
        ActionLogic.LogicFinished += OnLogicFinished;
        ActionLogic.LogicFailed += () => ActionFinished?.Invoke();
    }

    public abstract void Start();
    public abstract void Stop();
    public abstract void Update(float delta);

    private void OnLogicFinished()
    {
        ActionState.ApplyEffects();
        ActionFinished?.Invoke();
    }
}
