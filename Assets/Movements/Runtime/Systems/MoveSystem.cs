using Unity.Burst;
using Unity.Entities;

namespace Movements.Runtime.Systems
{
    public partial struct MoveSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var moveJobEntity = new global::Movements.Runtime.Systems.MoveJobEntity();
            moveJobEntity.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}