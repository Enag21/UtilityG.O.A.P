using System;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;

namespace UGOAP.Agent;

public partial class EatActionLogic : Node, IActionLogic
{
    public event Action LogicFinished;
    public event Action LogicFailed;

    private BaseAgent2D _agent;

    public EatActionLogic(IAgent agent) => _agent = (BaseAgent2D)agent;

    public void Start() { }

    public void Stop() { }

    public void Update(float delta)
    {
        _agent.State.ParameterManager.UpdateParameter(ParameterType.Hunger, 100.0f);
        LogicFinished?.Invoke();
    }
}