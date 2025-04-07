using Unity.Burst;
using Unity.Entities;

namespace Movements.Runtime.Systems
{
    public partial struct TargetTeleportSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var targetTeleportJobEntity = new global::Movements.Runtime.Systems.TargetTeleportJobEntity();
            targetTeleportJobEntity.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}