using Godot;
using UGOAP.AgentComponents;
using UGOAP.KnowledgeRepresentation.BeliefSystem;

namespace UGOAP;

[GlobalClass]
public partial class SensableComponent : Area2D, ISensable
{
    public IBeliefComponent BeliefComponent { get; private set; } = new BeliefComponent();

    public void AddBelief(Belief belief) => BeliefComponent.AddBelief(belief);

    public void UpdateBelief(Belief belief) => BeliefComponent.UpdateBelief(belief);
}
