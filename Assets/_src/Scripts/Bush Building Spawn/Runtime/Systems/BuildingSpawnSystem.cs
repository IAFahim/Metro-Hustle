using Unity.Burst;
using Unity.Entities;

namespace _src.Scripts.Bush_Building_Spawn.Runtime.Systems
{
    public partial struct BuildingSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            foreach (var buildingSpawnComponentData in SystemAPI.Query<RefRO<BuildingSpawnComponentData>>())
            {
                var prefab = buildingSpawnComponentData.ValueRO.FloorPrefab;
                ecb.Instantiate(prefab);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}