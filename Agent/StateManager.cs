using Godot;
using UGOAP.AgentComponents;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.Agent;

[GlobalClass]
public partial class StateManager : Node
{
    [Export] BeliefComponent _beliefComponent;
    [Export] TraitManager _traitManager;
    [Export] ParameterManager _parameterManager;

    public State State { get; private set; }

    public override void _Ready()
    {
        State = new State(_beliefComponent, _traitManager, _parameterManager);
    }
}