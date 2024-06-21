using UGOAP.BehaviourSystem.Planners;
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
            action.ActionState.Effects.ForEach(effect => effect.ApplyEffect(_simulatedState));
/*             foreach (var modifier in action.ParameterModifiers)
            {
                _simulatedState.ParameterManager.UpdateParameter(modifier);
            } */
        }
        return _simulatedState;
    }
}