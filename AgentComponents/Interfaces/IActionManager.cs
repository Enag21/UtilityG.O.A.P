using System.Collections.Generic;
using UGOAP.BehaviourSystem.Actions;

namespace UGOAP.AgentComponents.Interfaces;

public interface IActionManager
{
    HashSet<IAction> AvailableActions { get; }
    public void RegisterAction(IAction action);
    public void RemoveAction(IAction action);
}