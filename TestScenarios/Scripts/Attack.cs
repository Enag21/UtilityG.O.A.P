using Godot;
using UGOAP.KnowledgeRepresentation.BeliefSystem;

namespace UGOAP.TestScenarios.Scripts;

[GlobalClass]
public partial class Attack : Resource
{
    [Export] public float Damage { get; set; }
    [Export]public float CoolDown { get; set; }
    public IEntity Owner { get; set; }
}