using System.Collections.Generic;
using Godot;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;

namespace UGOAP.SmartObjects;

public interface ISmartObject
{
    FastName Id { get; }
    Vector2 Location { get; }
    HashSet<IActionBuilder> SuppliedActionBuilders { get; }
}