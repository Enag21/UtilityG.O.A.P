using System;
using System.Collections.Generic;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.SmartObjects;

namespace UGOAP.BehaviourSystem.Actions;

public interface IAction
{
    event Action ActionFinished;
    FastName ActionName { get; }
    ISmartObject Provider { get; }
    Effects Effects { get; }
    Preconditions Preconditions { get; }
    List<IParameterModifier> ParameterModifiers { get; }
    float Cost { get; }
    void Start();
    void Stop();
    void Update(float delta);
}