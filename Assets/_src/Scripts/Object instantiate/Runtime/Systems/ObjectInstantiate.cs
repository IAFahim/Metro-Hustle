using _src.Scripts.Object_instantiate.Runtime.Datas;
using BovineLabs.Core.Groups;
using BovineLabs.Core.ObjectManagement;
using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

namespace _src.Scripts.Object_instantiate.Runtime.Systems
{
    [BurstCompile]
    [UpdateInGroup(typeof(AfterSceneSystemGroup))]
    public partial struct ObjectInstantiate : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<ObjectDefinitionRegistry>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var ecb = new EntityCommandBuffer(state.WorldUpdateAllocator);
            var objectDefinitionRegistry = SystemAPI.GetSingleton<ObjectDefinitionRegistry>();
            foreach (var (spawnCommand, localTransform,enitiy) in SystemAPI.Query<RefRO<SpawnCommand>, RefRO<LocalTransform>>().WithEntityAccess())
            {
                var entityPrefab = objectDefinitionRegistry[spawnCommand.ValueRO.Prefab];
                var entity = ecb.Instantiate(entityPrefab);
                ecb.SetComponent(entity, LocalTransform.FromPosition(localTransform.ValueRO.Position));
                
            }
            ecb.Playback(state.EntityManager);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}