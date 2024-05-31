using UGOAP.KnowledgeRepresentation.PersonalitySystem;

namespace UGOAP.KnowledgeRepresentation.StateRepresentation;

public interface IState : IBeliefState, ITraitState, IParameterState, ICopyable<IState> { }