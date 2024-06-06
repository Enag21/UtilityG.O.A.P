using Godot;
using UGOAP.AgentComponents;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.Agent;

[GlobalClass]
public partial class StateManager : Node
{
    [Export] BeliefComponent beliefComponent;
    [Export] TraitManager traitManager;
    [Export] ParameterManager parameterManager;

    public State State { get; private set; }

    public override void _Ready()
    {
        State = new State(beliefComponent, traitManager, parameterManager);
    }
}