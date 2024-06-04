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
        var currentResult = GetBelief(belief.Predicate)?.Evaluate();

        Beliefs[belief.Predicate] = belief;

        GD.Print($"Updated belief {belief.Predicate} from {currentResult} to {Beliefs[belief.Predicate].Evaluate()}");
    }

    public void RemoveBelief(FastName predicate) => Beliefs.Remove(predicate);

    /// <summary>
    /// Retrieves a Belief object based on the given predicate.
    /// If the Belief does not exist, it creates a new Belief with false condition, and the given predicate and adds it to the Beliefs dictionary.
    /// </summary>
    /// <param name="predicate">The predicate used to retrieve the Belief.</param>
    /// <returns>The Belief object associated with the given predicate.</returns>
    public Belief GetBelief(FastName predicate)
    {
        var belief = Beliefs.GetValueOrDefault(predicate);
        if (belief == null)
        {
            var newBelief = new Belief.BeliefBuilder(predicate)
                .WithCondition(() => false)
                .Build();
            AddBelief(newBelief);
            return newBelief;
        }
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