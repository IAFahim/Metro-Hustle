#if Aline
using Drawing;
#endif
using Unity.Burst;
using Unity.Entities;

namespace Scripts.CollisionEffect.Editor
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.Editor| WorldSystemFilterFlags.Default)]
    internal partial struct DebugSystemCollisionEffect : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
#if Aline
            //var builder = DrawingManager.GetBuilder();
                
            //builder.Dispose();
#endif
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}