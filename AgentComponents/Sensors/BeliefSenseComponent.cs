using Godot;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.AgentComponents.Sensors;

[GlobalClass]
public partial class BeliefSenseComponent : SenseComponentBase
{
    public override void Update(ISensable sensable)
    {
        if (sensable is IBeliefState beliefState)
        {
            foreach (var (_, belief) in beliefState.BeliefComponent.Beliefs)
            {
                _agent.State.BeliefComponent.UpdateBelief(belief);
            }
        }
    }
}
