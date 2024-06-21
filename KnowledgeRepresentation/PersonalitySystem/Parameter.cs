using UGOAP.CommonUtils.Interfaces;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public class Parameter : IParameter
{
    public ParameterType Type { get; private set; }
    public float Value { get; set; }
    public Parameter(ParameterType type, float value) => (Type, Value) = (type, value);
    public IParameter Copy() => new Parameter(Type, Value);
}