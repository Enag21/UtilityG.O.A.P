using System;

namespace UGOAP.BehaviourSystem.Actions;

public interface IActionLogic
{
    event Action LogicFinished;
    event Action LogicFailed;
    void Start();
    void Stop();
    void Update(float delta);
}