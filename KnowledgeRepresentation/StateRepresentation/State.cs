using System.Collections.Generic;
using UGOAP.AgentComponents;
using UGOAP.CommonUtils.ExtensionMethods;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;

namespace UGOAP.KnowledgeRepresentation.StateRepresentation;

public class State : IState
{
    public IBeliefComponent BeliefComponent { get; private set; }
    public ITraitManager TraitManager { get; private set; }
    public IParameterManager ParameterManager { get; private set; }

    public State()
    {
        BeliefComponent = new BeliefComponent();
        TraitManager = new TraitManager();
        ParameterManager = new ParameterManager();
    }

    public State(
        IBeliefComponent beliefComponent,
        ITraitManager traitManager,
        IParameterManager parameterManager
    )
    {
        BeliefComponent = beliefComponent;
        TraitManager = traitManager;
        ParameterManager = parameterManager;
    }

    private State(IState state)
    {
        BeliefComponent = state.BeliefComponent.Copy();
        TraitManager = state.TraitManager;
        ParameterManager = state.ParameterManager.Copy();
    }

    public State(HashSet<Belief> stateEffects)
    {
        BeliefComponent = new BeliefComponent();
        TraitManager = new TraitManager();
        ParameterManager = new ParameterManager();
        stateEffects.ForEach(effect => BeliefComponent.AddBelief(effect));
    }

    public IState Copy() => new State(this);
}