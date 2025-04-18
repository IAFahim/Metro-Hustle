using _src.Scripts.Dimensions.Runtime.Datas;
using Drawing;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;


namespace _src.Scripts.Dimensions.Editor.Settings
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.Editor| WorldSystemFilterFlags.Default)]
    public partial struct Dimensions2DDebugSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
#if Aline
            var builder = DrawingManager.GetBuilder();
            foreach (var (localTransform, areaComponentData) in 
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<Dimensions2DComponentData>>())
            {
                float3 size = new float3(areaComponentData.ValueRO.Float2.x, 0, areaComponentData.ValueRO.Float2.y);
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