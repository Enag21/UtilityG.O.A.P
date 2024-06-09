namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public interface IParameterModifier
{
    ParameterType ParameterType { get; }
    void Modify(IParameter parameter);
}