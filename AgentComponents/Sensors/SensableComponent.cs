using Godot;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.AgentComponents.Sensors;

[GlobalClass]
public partial class SensableComponent : Area2D, ISensable, IBeliefState
{
    public IBeliefComponent BeliefComponent { get; private set; } = new BeliefComponent();

    public void AddBelief(Belief belief) => BeliefComponent.AddBelief(belief);

    public void UpdateBelief(Belief belief) => BeliefComponent.UpdateBelief(belief);
}
