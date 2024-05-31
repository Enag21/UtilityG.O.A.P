using UGOAP.CommonUtils.FastName;
using UGOAP.CommonUtils.Interfaces;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public class Parameter : IParameter
{
    public FastName Name { get; }
    public float Value { get; private set; }
    public Parameter(FastName name, float value) => (Name, Value) = (name, value);
    public void Update(IUpdateStrategy updateStrategy) => Value = updateStrategy.Update(Value);
    public IParameter Copy() => new Parameter(Name, Value);
}