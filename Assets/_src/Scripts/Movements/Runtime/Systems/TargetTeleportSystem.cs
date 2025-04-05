using Unity.Burst;
using Unity.Entities;

namespace _src.Scripts.Movements.Runtime.Systems
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
            var targetTeleportJobEntity = new TargetTeleportJobEntity();
            targetTeleportJobEntity.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}