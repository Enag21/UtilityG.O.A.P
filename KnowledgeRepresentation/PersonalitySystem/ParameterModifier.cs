using UGOAP.CommonUtils.FastName;
using UGOAP.CommonUtils.Interfaces;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public class ParameterModifier : IParameterModifier
{
    public FastName ParameterName { get; set; }
    private IUpdateStrategy _updateStrategy;

    public ParameterModifier(IUpdateStrategy updateStrategy) => _updateStrategy = updateStrategy;

    public void Modify(IParameter parameter) => parameter.Update(_updateStrategy);
}