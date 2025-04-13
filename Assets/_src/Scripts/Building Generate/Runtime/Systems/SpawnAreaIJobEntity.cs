using _src.Scripts.Building_Generate.Runtime.Datas;
using _src.Scripts.Building_Generate.Runtime.Gen;
using Unity.Collections;
using Unity.Entities;

namespace _src.Scripts.Building_Generate.Runtime.Systems
{
    public partial struct SpawnAreaIJobEntity : IJobEntity
    {
        public EntityCommandBuffer.ParallelWriter ECB;
        public BufferLookup<GroundFloorBuffer> GroundFloorBufferLookup;

        private void Execute(
            Entity entity, [EntityIndexInChunk] int entityIndexInChunk,
            ref SpawnCountComponentData spawnCountComponentData,
            in AreaSpawnerComponentData areaSpawnerComponentData
        )
        {
            if (spawnCountComponentData.Count <= 0) return;
            var groundFloorBuffers = GroundFloorBufferLookup[entity];
            ECB.Instantiate(entityIndexInChunk, groundFloorBuffers[0].Prefab);
            spawnCountComponentData.Count--;
        }
    }
}