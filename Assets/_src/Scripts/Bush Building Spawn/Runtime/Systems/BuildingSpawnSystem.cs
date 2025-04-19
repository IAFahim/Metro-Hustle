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
            foreach (var buildingSpawnComponentData in SystemAPI.Query<RefRW<BuildingSpawnComponentData>>())
            {
                if (buildingSpawnComponentData.ValueRO.Count < buildingSpawnComponentData.ValueRO.HighestCount)
                {
                    var prefab = buildingSpawnComponentData.ValueRO.FloorPrefab;
                    ecb.Instantiate(prefab);
                    buildingSpawnComponentData.ValueRW.Count++;
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}