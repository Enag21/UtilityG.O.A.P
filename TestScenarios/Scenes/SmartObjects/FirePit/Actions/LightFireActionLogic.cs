using System;
using Godot;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.SmartObjects;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;

namespace UGOAP;

public partial class LightFireActionLogic : Node, IActionLogic
{
    public event Action LogicFinished;
    private FirePit _firePit;
    private IAgent _agent;

    public LightFireActionLogic(ISmartObject firePit, IAgent agent) => (_firePit, _agent) = (firePit as FirePit, agent);

    public void Update(float delta)
    {
        if (!_firePit.IsLit)
        {
            _firePit.LightFire();
            _agent.State.BeliefComponent.UpdateBelief(new Belief.BeliefBuilder(Facts.Effects.HasWood).WithCondition(() => false).Build());
            LogicFinished?.Invoke();
        }
    }

    public void Start() { }

    public void Stop() { }
}
