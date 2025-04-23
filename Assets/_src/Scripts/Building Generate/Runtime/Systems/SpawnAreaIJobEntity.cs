using System.Numerics;
using _src.Scripts.Building_Generate.Runtime.Datas;
using _src.Scripts.Building_Generate.Runtime.Gen;
using _src.Scripts.Dimensions.Runtime.Datas;
using BovineLabs.Core.Entropy;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _src.Scripts.Building_Generate.Runtime.Systems
{
    public partial struct SpawnAreaIJobEntity : IJobEntity
    {
        [WriteOnly] public EntityCommandBuffer.ParallelWriter ECB;
        [ReadOnly] public BufferLookup<GroundFloorBuffer> GroundFloorBufferLookup;

        private void Execute(
            Entity entity, [EntityIndexInChunk] int entityIndexInChunk,
            ref SpawnGapAndCountComponentData spawnGapAndCountComponentData,
            in LocalTransform localTransform,
            in Dimensions2DComponentData dimensions2D
        )
        {
            if (spawnGapAndCountComponentData.Count <= 0) return;
            var groundFloorBuffers = GroundFloorBufferLookup[entity];
            var xRand = GlobalRandom.NextFloat();
            var zRand = GlobalRandom.NextFloat();
            var x = xRand * spawnGapAndCountComponentData.Gap + dimensions2D.Float2.x;
            var z = zRand * spawnGapAndCountComponentData.Gap + dimensions2D.Float2.y;
            var count = spawnGapAndCountComponentData.Count;
            for (int i = 0; i < count; i++)
            {
                var position = new float3(x, i, z);
                var randomUp = GlobalRandom.NextFloat() * 10;

                var randomIndex = GlobalRandom.NextInt(groundFloorBuffers.Length);
                var randomGroundFloor = groundFloorBuffers[randomIndex];

                var createdEntity = ECB.Instantiate(entityIndexInChunk, randomGroundFloor.Entity);
                var transform = new LocalTransform()
                {
                    Position = position + localTransform.Position,
                    Rotation = quaternion.EulerXYZ(0, randomUp, 0),
                    Scale = 1
                };
                ECB.AddComponent(entityIndexInChunk, createdEntity, transform);
                
                var float4X4 = transform.ToMatrix();
                float4X4.c0.x = randomGroundFloor.Scale.x;
                float4X4.c1.y = randomGroundFloor.Scale.y;
                float4X4.c2.z = randomGroundFloor.Scale.z;
                ECB.AddComponent(entityIndexInChunk, createdEntity, new PostTransformMatrix()
                {
                    Value = float4X4
                });
            }
            spawnGapAndCountComponentData.Count = 0;
        }
    }
}