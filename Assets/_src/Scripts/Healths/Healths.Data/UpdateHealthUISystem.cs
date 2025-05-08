// In your ECS systems assembly (e.g., YourProject.Systems)

using BovineLabs.Anchor;
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

        public void OnStopRunning(ref SystemState state)
        {
            this._uiHelper.Unbind();
        }
    }
}