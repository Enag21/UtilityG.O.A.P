using System.Collections.Generic;
using Godot;
using UGOAP.Agent;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.BeliefSystem;
using UGOAP.KnowledgeRepresentation.Facts;
using UGOAP.SmartObjects;
using UGOAP.TestScenarios.Components;
using UGOAP.TestScenarios.Scenes.SmartObjects.Weapon;

namespace UGOAP.AgentComponents.Sensors;

[GlobalClass]
public partial class TargetSenseComponent : SenseComponentBase
{
    [Export] SmartWeapon _weapon;
    public override void Update(ISensable sensable)
    {
        if (sensable is IDamagable target)
        {
            if (target is IEntity entity)
            {
                var targetBelief = new Belief.BeliefBuilder(new FastName($"Target {entity.Id}"))
                    .WithEntity(new EntityFluent.AboutEntity(entity).WithHealth(entity.Data[Names.Health]).Create())
                    .Build();
                _agent.State.BeliefComponent.UpdateBelief(targetBelief);
                _weapon.RegisterTarget(target);
            }
        }
    }
}