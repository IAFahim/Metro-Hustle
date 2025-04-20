using _src.Scripts.Building_Generate.Runtime.Datas;
using _src.Scripts.Building_Generate.Runtime.Gen;
using _src.Scripts.Dimensions.Runtime.Datas;
using BovineLabs.Core.Entropy;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Systems
{
    public partial struct SpawnAreaIJobEntity : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public BufferLookup<GroundFloorBuffer> GroundFloorBufferLookup;

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
            for (int i = 0; i < count; i--)
            {
                var position = new float3(x, i, z);
                Debug.Log(position);
                var randomUp = GlobalRandom.NextFloat() * 10;

                var groundFloorIndex = GlobalRandom.NextInt(groundFloorBuffers.Length);
                var groundFloor = groundFloorBuffers[groundFloorIndex];

                var createdEntity = ECB.Instantiate(entityIndexInChunk, groundFloor.Prefab);
                var positionAndRotation = new LocalTransform()
                {
                    Position = position + localTransform.Position,
                    Rotation = quaternion.EulerXYZ(0, randomUp, 0),
                    Scale = 1
                };
                ECB.AddComponent(entityIndexInChunk, createdEntity, positionAndRotation);
                spawnGapAndCountComponentData.Count--;
            }
        }
    }
}