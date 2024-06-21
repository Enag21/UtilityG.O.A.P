using System;
using System.Collections.Generic;
using UGOAP.Agent;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public interface IAction
{
    event Action ActionFinished;
    IActionLogic ActionLogic { get; set; }
    IActionState ActionState { get; set; }
    void Start();
    void Stop();
    void Update(float delta);
}

public interface IActionState
{
    FastName ActionName { get; set; }
    List<IEffect> Effects { get; }
    Preconditions Preconditions { get; }
    Func<float> Cost { get; set; }
    ISmartObject Provider { get; set; }
    IAgent User { get; set; }
    void ApplyEffects();
}

public class ActionState : IActionState
{
    public FastName ActionName { get; set;}
    public List<IEffect> Effects { get; private set; } = new();
    public Preconditions Preconditions { get; private set; } = new();
    public ISmartObject Provider { get; set;}
    public Func<float> Cost { get; set; } = () => 1.0f;
    public IAgent User { get; set; }

    public void ApplyEffects()
    {
        Effects.ForEach(effect => effect.ApplyEffect(User.State));
    }
}