using _src.Scripts.Animations.Animations.Data;
using Unity.Entities;
using Unity.Scenes;
using UnityEngine;

namespace _src.Scripts.Animations.Animations
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct InstantiateAnimatorSystem : ISystem
    {
        public void OnUpdate(ref SystemState state)
        {
            var ecb = SystemAPI.GetSingleton<EndInitializationEntityCommandBufferSystem.Singleton>()
                .CreateCommandBuffer(state.WorldUnmanaged);

            foreach (
                var (animationComponent, entity) in
                SystemAPI.Query<PrefabComponentClass>().WithEntityAccess()
            )
            {
                var gameObject = Object.Instantiate(animationComponent.Prefab);
                ecb.AddComponent(entity, new AnimatorComponent()
                {
                    Ref = gameObject.GetComponent<Animator>(),
                    State = 0
                });
                ecb.RemoveComponent<PrefabComponentClass>(entity);
            }
        }
    }
}