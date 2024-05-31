using System.Collections.Generic;
using UGOAP.CommonUtils.FastName;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public interface IParameterManager : ICopyable<IParameterManager>
{
    Dictionary<FastName, IParameter> Parameters { get; }
    IParameter GetParameter(FastName name);
    void AddParameter(IParameter parameter);
    void RemoveParameter(FastName name);
    void UpdateParameter(IParameterModifier modifier);
}