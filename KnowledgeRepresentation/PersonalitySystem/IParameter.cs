using UGOAP.CommonUtils.FastName;
using UGOAP.CommonUtils.Interfaces;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public interface IParameter : ICopyable<IParameter>
{
    FastName Name { get; }
    float Value { get; }
    void Update(IUpdateStrategy updateStrategy);
}