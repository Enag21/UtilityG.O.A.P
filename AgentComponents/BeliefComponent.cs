using System.Collections.Generic;
using Godot;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;

namespace UGOAP.AgentComponents;

public partial class BeliefComponent : Node, IBeliefComponent
{
    public Dictionary<FastName, Belief> Beliefs { get; private set; } =
        new Dictionary<FastName, Belief>();

    public void AddBelief(Belief belief) => Beliefs.Add(belief.Predicate, belief);

    public void UpdateBelief(Belief belief)
    {
        // For debugging
        var updatedResult = belief.Evaluate();
        var currentResult = GetBelief(belief.Predicate).Evaluate();

        Beliefs[belief.Predicate] = belief;

        GD.Print($"Updated belief {belief.Predicate} from {currentResult} to {Beliefs[belief.Predicate].Evaluate()}");
    }

    public void RemoveBelief(FastName predicate) => Beliefs.Remove(predicate);

    public Belief GetBelief(FastName predicate)
    {
        var belief = Beliefs.GetValueOrDefault(predicate);
        return belief;
    }

    public IBeliefComponent Copy()
    {
        var clonedBeliefs = new Dictionary<FastName, Belief>();
        foreach (var (predicate, belief) in Beliefs)
        {
            clonedBeliefs.Add(predicate, belief.Copy());
        }
        return new BeliefComponent { Beliefs = clonedBeliefs };
    }

    public void PrintBeliefs()
    {
        foreach (var (predicate, belief) in Beliefs)
        {
            GD.Print($"{predicate} = {belief.Evaluate()}");
        }
    }
}