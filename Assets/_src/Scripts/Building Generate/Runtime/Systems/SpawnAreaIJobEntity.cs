using _src.Scripts.Building_Generate.Runtime.Datas;
using _src.Scripts.Building_Generate.Runtime.Gen;
using BovineLabs.Core.Entropy;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _src.Scripts.Building_Generate.Runtime.Systems
{
    public partial struct SpawnAreaIJobEntity : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public BufferLookup<GroundFloorBuffer> GroundFloorBufferLookup;

        private void Execute(
            Entity entity, [EntityIndexInChunk] int entityIndexInChunk,
            ref SpawnCountComponentData spawnCountComponentData,
            in LocalTransform localTransform,
            in AreaSpawnerComponentData areaSpawnerComponentData
        )
        {
            if (spawnCountComponentData.Count <= 0) return;
            var groundFloorBuffers = GroundFloorBufferLookup[entity];
            for (int i = 0; i < spawnCountComponentData.Count; i++)
            {
                var randomPosition3 = GlobalRandom.NextFloat2();
                var x = randomPosition3.x * areaSpawnerComponentData.Area.x;
                var y = localTransform.Position.y;
                var z = randomPosition3.y * areaSpawnerComponentData.Area.y;
                var position = new float3(x, y, z);
                var randomUp = GlobalRandom.NextFloat() * 10;

                var groundFloorIndex = GlobalRandom.NextInt(groundFloorBuffers.Length);
                var groundFloor = groundFloorBuffers[groundFloorIndex];

                var createdEntity = ECB.Instantiate(entityIndexInChunk, groundFloor.Prefab);
                var randomPositionAndRoation = new LocalTransform()
                {
                    Position=position,
                    Rotation = quaternion.EulerXYZ(0, randomUp, 0),
                    Scale = 1
                };
                ECB.AddComponent(entityIndexInChunk, createdEntity, randomPositionAndRoation);
                spawnCountComponentData.Count--;
            }
        }
    }
}