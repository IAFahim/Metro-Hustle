using _src.Scripts.Building_Generate.Runtime.Datas;
using BovineLabs.Core.ObjectManagement;
using Unity.Burst;
using Unity.Entities;

namespace _src.Scripts.Building_Generate.Runtime.Systems
{
    [UpdateBefore(typeof(EndSimulationEntityCommandBufferSystem))]
    public partial struct SpawnSystem : ISystem
    {
        private BufferLookup<GroundFloorBuffer> _groundFloorBufferLookup;

        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ObjectDefinitionRegistry>();
            _groundFloorBufferLookup = state.GetBufferLookup<GroundFloorBuffer>(true);
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            _groundFloorBufferLookup.Update(ref state);
            SpawnAreaIJobEntity spawnAreaIJobEntity = new SpawnAreaIJobEntity
            {
                GroundFloorBufferLookup = _groundFloorBufferLookup,
                ECB = ecb.AsParallelWriter(),
            };
            spawnAreaIJobEntity.Schedule();
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}