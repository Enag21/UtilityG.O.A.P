﻿using System;
using System.Collections.Generic;
using Godot;
using UGOAP.AgentComponents;
using UGOAP.AgentComponents.Interfaces;
using UGOAP.BehaviourSystem.Actions;
using UGOAP.CommonUtils.FastName;
using UGOAP.KnowledgeRepresentation.PersonalitySystem;
using UGOAP.KnowledgeRepresentation.StateRepresentation;

namespace UGOAP.Agent;

[GlobalClass]
public partial class BaseAgent2D : CharacterBody2D, IAgent
{
    [Export] public MoveComponent MoveComponent { get; private set; }
    [Export] public BehaviourComponent BehaviourComponent { get; private set; }
    public FastName Id { get; private set; }
    public Vector2 Location => GlobalPosition;
    public HashSet<IActionBuilder> SuppliedActionBuilders { get; private set; } = new();
    public INavigationComponent NavigationComponent => MoveComponent;
    public IState State { get; private set; } = new State(new BeliefComponent(), new TraitManager(), new ParameterManager());

    public override void _Ready()
    {
        Id = new FastName(Name);
    }
}