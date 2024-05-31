using UGOAP.Agent;

namespace UGOAP.BehaviourSystem.Actions;

public interface IActionBuilder
{
    IAction Build(IAgent agent);
}