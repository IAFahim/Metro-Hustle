// In your ECS systems assembly (e.g., YourProject.Systems)

using System.Collections.Generic;
using System.Linq;
using _src.Scripts.Healths.Healths.Data;
using BovineLabs.Anchor;
using BovineLabs.Stats.Data; // For Intrinsic, IntrinsicKey, IntrinsicData
using BovineLabs.Stats.Authoring; // For implicit conversion from schema object
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

[UpdateInGroup(typeof(UISystemGroup))]
public partial struct UpdateHealthUISystem : ISystem, ISystemStartStop
{
    private UIHelper<HealthViewModel, HealthViewModel.Data> uiHelper;


    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        this.uiHelper = new UIHelper<HealthViewModel, HealthViewModel.Data>(
            ref state, ComponentType.ReadOnly<GameScreenTag>());
    }

    public void OnStartRunning(ref SystemState state)
    {
        this.uiHelper.Bind();
    }

    public void OnStopRunning(ref SystemState state)
    {
        this.uiHelper.Unbind();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        ref var binding = ref this.uiHelper.Binding;
        binding.CurrentHealth = 100;
        binding.MaxHealth = 100;
    }
    
}