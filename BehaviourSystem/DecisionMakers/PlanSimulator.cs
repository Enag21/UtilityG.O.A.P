using UGOAP.AgentComponents;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.BehaviourSystem.DecisionMakers;

public class PlanSimulator
{
    private readonly IState _simulatedState;
    private readonly Plan _plan;

    public PlanSimulator(IState originalState, Plan plan)
    {
        _plan = plan;
        _simulatedState = originalState.Copy();
    }

    public IState Simulate()
    {
        foreach (var action in _plan.Actions)
        {
            action.Effects.ApplyEffects(_simulatedState);
            foreach (var modifier in action.ParameterModifiers)
            {
                _simulatedState.ParameterManager.UpdateParameter(modifier);
            }
        }
        return _simulatedState;
    }
}