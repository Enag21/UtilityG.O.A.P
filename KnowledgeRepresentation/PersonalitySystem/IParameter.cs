using UGOAP.CommonUtils.Interfaces;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public enum ParameterType
{
    Health,
    Hunger,
    Thirst,
    Energy,
    None
}
public interface IParameter : ICopyable<IParameter>
{
    ParameterType Type { get; }
    float Value { get; }
    void Update(IUpdateStrategy updateStrategy);
}