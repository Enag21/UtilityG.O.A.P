using UGOAP.CommonUtils.Interfaces;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public class ParameterModifier : IParameterModifier
{
    private IUpdateStrategy _updateStrategy;

    public ParameterModifier(IUpdateStrategy updateStrategy) => _updateStrategy = updateStrategy;
    public ParameterType ParameterType => throw new System.NotImplementedException();
}