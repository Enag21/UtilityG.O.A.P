using System.Collections.Generic;
using UGOAP.CommonUtils.FastName;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

public class ParameterManager : IParameterManager
{
    public Dictionary<FastName, IParameter> Parameters { get; }
    public ParameterManager() => Parameters = new Dictionary<FastName, IParameter>();
    public ParameterManager(IParameterManager parameterManager)
    {
        Parameters = new Dictionary<FastName, IParameter>();

        foreach (var kvp in parameterManager.Parameters)
        {
            Parameters.Add(kvp.Key, kvp.Value.Copy());
        }
    }

    public void AddParameter(IParameter parameter) => Parameters.Add(parameter.Name, parameter);

    public void RemoveParameter(FastName name) => Parameters.Remove(name);

    public IParameter GetParameter(FastName name) =>
        Parameters.GetValueOrDefault(name);

    public void UpdateParameter(IParameterModifier modifier)
    {
        var parameterToUpdate = Parameters.GetValueOrDefault(modifier.ParameterName);
        if (parameterToUpdate == null)
            return;
        modifier.Modify(parameterToUpdate);
    }

    public IParameterManager Copy()
    {
        return new ParameterManager(this);
    }
}