using _src.Scripts.Building_Generate.Runtime.Datas;
using _src.Scripts.Building_Generate.Runtime.Gen;
using _src.Scripts.Dimensions.Runtime.Datas;
using BovineLabs.Core.Entropy;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _src.Scripts.Building_Generate.Runtime.Systems
{
    [BurstCompile]
    public partial struct SpawnAreaIJobEntity : IJobEntity
    {
        [WriteOnly] public EntityCommandBuffer.ParallelWriter ECB;
        [ReadOnly] public BufferLookup<GroundFloorBuffer> GroundFloorBufferLookup;

        private void Execute(
            Entity entity, [EntityIndexInChunk] int entityIndexInChunk,
            ref SpawnHeightAndCountComponentData spawnHeightAndCountComponentData,
            in LocalTransform localTransform,
            in Dimensions2DComponentData dimensions
        )
        {
            if (spawnHeightAndCountComponentData.Count <= 0) return;
            var groundFloorBuffers = GroundFloorBufferLookup[entity];

            var count = spawnHeightAndCountComponentData.Count;
            for (int _ = 0; _ < count; _++)
            {
                var x = GlobalRandom.NextFloat() + dimensions.Value.x;
                var z = GlobalRandom.NextFloat() + dimensions.Value.y;
                var position = new float3(x, localTransform.Position.y, z);
                var randomIndex = GlobalRandom.NextInt(groundFloorBuffers.Length);
                var randomGroundFloor = groundFloorBuffers[randomIndex];

                var createdEntity = ECB.Instantiate(entityIndexInChunk, randomGroundFloor.Entity);
                var transform = new LocalTransform()
                {
                    Position = position + localTransform.Position,
                    Rotation = localTransform.Rotation,
                    Scale = 1
                };
                ECB.AddComponent(entityIndexInChunk, createdEntity, new LocalToWorld()
                {
                    Value = transform.ToMatrix()
                });

                var scaleX =
                    GlobalRandom.NextFloat(randomGroundFloor.ScaleRange.c0.x, randomGroundFloor.ScaleRange.c1.x);
                var scaleY =
                    GlobalRandom.NextFloat(randomGroundFloor.ScaleRange.c0.y, randomGroundFloor.ScaleRange.c1.y);
                var scaleZ =
                    GlobalRandom.NextFloat(randomGroundFloor.ScaleRange.c0.z, randomGroundFloor.ScaleRange.c1.z);

                var float4X4 = new float4x4()
                {
                    c0 = new float4(scaleX, 0, 0, 0),
                    c1 = new float4(0, scaleY, 0, 0),
                    c2 = new float4(0, 0, scaleZ, 0),
                    c3 = new float4(0, 0, 0, 1),
                };

                ECB.AddComponent(entityIndexInChunk, createdEntity, new PostTransformMatrix()
                {
                    Value = float4X4
                });
            }

            spawnHeightAndCountComponentData.Count = 0;
        }
    }
}