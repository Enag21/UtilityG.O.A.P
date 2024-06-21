using System.Collections.Generic;
using Godot;

namespace UGOAP.KnowledgeRepresentation.PersonalitySystem;

[GlobalClass]
public partial class ParameterManager : Node, IParameterManager
{
    public Dictionary<ParameterType, IParameter> Parameters { get; }
    public ParameterManager() => Parameters = new Dictionary<ParameterType, IParameter>();
    public ParameterManager(IParameterManager parameterManager)
    {
        Parameters = new Dictionary<ParameterType, IParameter>();

        foreach (var kvp in parameterManager.Parameters)
        {
            Parameters.Add(kvp.Key, kvp.Value.Copy());
        }
    }

    public void AddParameter(IParameter parameter) => Parameters.Add(parameter.Type, parameter);

    public void RemoveParameter(ParameterType name) => Parameters.Remove(name);

    public IParameter GetParameter(ParameterType name) =>
        Parameters.GetValueOrDefault(name);

    public void UpdateParameter(ParameterType parameter, float value)
    {
        var parameterToUpdate = Parameters.GetValueOrDefault(parameter);
        if (parameterToUpdate == null)
            return;
        parameterToUpdate.Value = Mathf.Clamp(parameterToUpdate.Value + value, 0.0f, 100.0f);
    }

    public IParameterManager Copy()
    {
        return new ParameterManager(this);
    }
}