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

    public void UpdateParameter(IParameterModifier modifier)
    {
        var parameterToUpdate = Parameters.GetValueOrDefault(modifier.ParameterType);
        if (parameterToUpdate == null)
            return;
        modifier.Modify(parameterToUpdate);
    }

    public IParameterManager Copy()
    {
        return new ParameterManager(this);
    }
}