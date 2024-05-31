using UGOAP.AgentComponents;

namespace UGOAP.KnowledgeRepresentation.StateRepresentation;

public interface IBeliefState
{
    IBeliefComponent BeliefComponent { get; }
}