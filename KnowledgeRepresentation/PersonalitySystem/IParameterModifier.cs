using UGOAP.CommonUtils.FastName;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public interface IParameterModifier
{
    FastName ParameterName { get; }
    void Modify(IParameter parameter);
}