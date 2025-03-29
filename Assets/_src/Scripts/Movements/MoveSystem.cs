using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace _src.Scripts.Movements
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
            var moveJobEntity = new MoveJobEntity();
            moveJobEntity.ScheduleParallel();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}