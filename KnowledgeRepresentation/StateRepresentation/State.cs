using System.Collections.Generic;
using UGOAP.AgentComponents;
using UGOAP.BehaviourSystem.Planners;
using UGOAP.CommonUtils.ExtensionMethods;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;

namespace UGOAP.KnowledgeRepresentation.StateRepresentation;

public class State : IState
{
    public IBeliefComponent BeliefComponent { get; private set; }
    public ITraitManager TraitManager { get; private set; }
    public IParameterManager ParameterManager { get; private set; }

    public State(TraitManager traitManager)
    {
        BeliefComponent = new BeliefComponent();
        TraitManager = traitManager;
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

    public State(List<IEffect> stateEffects)
    {
        BeliefComponent = new BeliefComponent();
        TraitManager = new TraitManager();
        ParameterManager = new ParameterManager();
        foreach (var effect in stateEffects)
        {
            if (effect is BeliefEffect beliefEffect)
            {
                BeliefComponent.AddBelief(beliefEffect.Effect.Copy());
            }
            else if (effect is FluentEffect fluentEffect)
            {
                BeliefComponent.AddBelief(new Belief.BeliefBuilder(new FastName($"About {fluentEffect.EntityFluent.Entity.Id}"))
                    .WithEntity(fluentEffect.EntityFluent).Build());
            }
        }
    }

    public IState Copy() => new State(this);
}