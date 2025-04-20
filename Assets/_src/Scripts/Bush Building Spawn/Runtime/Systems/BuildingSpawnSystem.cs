using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _src.Scripts.Bush_Building_Spawn.Runtime.Systems
{
    [UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct BuildingSpawnSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
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
                    for (int i = 0; i < buildingSpawnComponentData.ValueRO.HighestCount; i++)
                    {
                        var prefab = buildingSpawnComponentData.ValueRO.FloorPrefab;
                        ecb.Instantiate(prefab);
                        var localTransform = new LocalTransform()
                        {
                            Position = new float3(0, i, 0),
                            Scale = 1
                        };
                        ecb.AddComponent(prefab,localTransform);
                    }

                    buildingSpawnComponentData.ValueRW.Count = buildingSpawnComponentData.ValueRO.HighestCount;
                }
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}