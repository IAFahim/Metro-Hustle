using _src.Scripts.Dimensions.Runtime.Datas;
using Drawing;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


namespace _src.Scripts.Dimensions.Editor.Settings
{
    [BurstCompile]
    [WorldSystemFilter(WorldSystemFilterFlags.Editor)]
    public partial struct DimensionsDebugSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
#if Aline
            var builder = DrawingManager.GetBuilder();
            foreach (var (localTransform, dimensions) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<Dimensions2DComponentData>>())
            {
                float3 size = new float3(dimensions.ValueRO.Value.x, 0, dimensions.ValueRO.Value.y);
                builder.WireBox(localTransform.ValueRO.Position, localTransform.ValueRO.Rotation, size);
            }

            foreach (var (localTransform, dimensions) in
                     SystemAPI.Query<RefRO<LocalTransform>, RefRO<Dimensions3DComponentData>>())
            {
                float3 scale = dimensions.ValueRO.Value;
                var position = localTransform.ValueRO.Position;
                var rotation = localTransform.ValueRO.Rotation;
                builder.WireBox(position, rotation, scale);
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