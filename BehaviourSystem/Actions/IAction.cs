using System;
using System.Collections.Generic;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public interface IAction
{
    event Action ActionFinished;
    CommonUtils.FastName.FastName ActionName { get; }
    ISmartObject Provider { get; }
    Effects Effects { get; }
    Preconditions Preconditions { get; }
    List<IParameterModifier> ParameterModifiers { get; }
    float Cost { get; }
    void Start();
    void Stop();
    void Update(float delta);
}