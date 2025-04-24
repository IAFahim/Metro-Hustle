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
                var randomIndex = GlobalRandom.NextInt(groundFloorBuffers.Length);
                var randomGroundFloor = groundFloorBuffers[randomIndex];
                var createdEntity = ECB.Instantiate(entityIndexInChunk, randomGroundFloor.Entity);
                CalculatePosition(localTransform, dimensions, out var position);
                SetLocalTransformAndLocalToWorld(entityIndexInChunk, createdEntity, localTransform, position);
                SetPostTransformScale(entityIndexInChunk, createdEntity, randomGroundFloor);
            }
            ECB.DestroyEntity(entityIndexInChunk, entity);
        }

        [BurstCompile]
        private static void CalculatePosition(
            in LocalTransform localTransform,
            in Dimensions2DComponentData dimensions,
            out float3 position
        )
        {
            var x = GlobalRandom.NextFloat() + dimensions.Value.x;
            var z = GlobalRandom.NextFloat() + dimensions.Value.y;
            position = new float3(x, localTransform.Position.y, z);
        }

        [BurstCompile]
        private void SetLocalTransformAndLocalToWorld(
            int entityIndexInChunk, in Entity createdEntity,
            in LocalTransform localTransform, float3 position
        )
        {
            var transform = new LocalTransform()
            {
                Position = position + localTransform.Position,
                Rotation = localTransform.Rotation,
                Scale = 1
            };
            ECB.AddComponent(entityIndexInChunk, createdEntity, transform);
            ECB.AddComponent(entityIndexInChunk, createdEntity, new LocalToWorld
            {
                Value = transform.ToMatrix()
            });
        }

        [BurstCompile]
        private void SetPostTransformScale(
            int entityIndexInChunk, in Entity createdEntity,
            GroundFloorBuffer randomGroundFloor
        )
        {
            var scaleX = GlobalRandom.NextFloat(1, randomGroundFloor.ScaleScaler.x);
            var scaleY = GlobalRandom.NextFloat(1, randomGroundFloor.ScaleScaler.y);
            var scaleZ = GlobalRandom.NextFloat(1, randomGroundFloor.ScaleScaler.z);

            var float4X4 = new float4x4
            {
                c0 = new float4(scaleX, 0, 0, 0),
                c1 = new float4(0, scaleY, 0, 0),
                c2 = new float4(0, 0, scaleZ, 0),
                c3 = new float4(0, 0, 0, 1),
            };

            ECB.AddComponent(entityIndexInChunk, createdEntity, new PostTransformMatrix
            {
                Value = float4X4
            });
        }
    }
}