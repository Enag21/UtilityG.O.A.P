using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP;

public interface ISensor
{
    void UpdateBeliefs(ISensable sensable);
}

public interface ISensable : IBeliefState
{
}

