using System.Collections.Generic;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public interface IParameterManager : ICopyable<IParameterManager>
{
    Dictionary<ParameterType, IParameter> Parameters { get; }
    IParameter GetParameter(ParameterType name);
    void AddParameter(IParameter parameter);
    void RemoveParameter(ParameterType name);
    void UpdateParameter(IParameterModifier modifier);
}