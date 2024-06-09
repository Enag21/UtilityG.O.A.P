using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.AgentComponents.Interfaces;

public interface ISensor
{
    void UpdateBeliefs(ISensable sensable);
}

public interface ISensable : IBeliefState
{
}

