// In your ECS systems assembly (e.g., YourProject.Systems)

using BovineLabs.Anchor;
using BovineLabs.Core.States;
using Unity.Burst;
using Unity.Entities;

namespace _src.Scripts.Healths.Healths.Data
{
    [UpdateInGroup(typeof(UISystemGroup))]
    public partial struct UpdateHealthUISystem : ISystem, ISystemStartStop
    {
        private UIHelper<HealthViewModel, HealthViewModel.Data> _uiHelper;

        public void OnStartRunning(ref SystemState state)
        {
            this._uiHelper.Bind();
        }

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            StateAPI.Register<HealthViewModel.Data, HealthState>(ref state, 1);
        }

        public void OnStopRunning(ref SystemState state)
        {
            this._uiHelper.Unbind();
        }

        private struct HealthState : IComponentData
        {
            public float Value;
        }
    }
}