using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.AgentComponents.Interfaces;

public interface ISensor
{
    void Update(ISensable sensable);
}

public interface ISensable
{
}

