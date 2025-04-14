using _src.Scripts.Areas.Runtime.Datas;
using Drawing;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _src.Scripts.Areas.Editor
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.Editor| WorldSystemFilterFlags.Default)]
    public partial struct SizeDebugSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
#if Aline
            var builder = DrawingManager.GetBuilder();
            foreach (var (localTransform, areaComponentData) in SystemAPI
                         .Query<RefRO<LocalTransform>, RefRO<AreaComponentData>>())
            {
                float3 size = new float3(areaComponentData.ValueRO.Value.x, 0, areaComponentData.ValueRO.Value.x);
                builder.WireBox(localTransform.ValueRO.Position, localTransform.ValueRO.Rotation, size);
            }

            builder.Dispose();
#endif
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}