using _src.Scripts.Object_instantiate.Runtime.Datas;
using BovineLabs.Core.ObjectManagement;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace _src.Scripts.Object_instantiate.Runtime.Systems
{
    [BurstCompile]
    public partial struct ObjectInstantiate : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<EndSimulationEntityCommandBufferSystem.Singleton>();
            state.RequireForUpdate<ObjectDefinitionRegistry>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);
            var objectDefinitionRegistry = SystemAPI.GetSingleton<ObjectDefinitionRegistry>();
            foreach (var (spawnCommand, localTransform) in SystemAPI.Query<RefRO<SpawnCommand>, RefRO<LocalTransform>>())
            {
                var entityPrefab = objectDefinitionRegistry[spawnCommand.ValueRO.Prefab];
                var entity = ecb.Instantiate(entityPrefab);
                ecb.SetComponent(entity, LocalTransform.FromPosition(localTransform.ValueRO.Position));
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}