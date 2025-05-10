using _src.Scripts.Animations.Animations.Data;
using BovineLabs.Core.Input;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace _src.Scripts.Animations.Animations
{
    [UpdateInGroup(typeof(PresentationSystemGroup))]
    public partial struct SyncAnimationData : ISystem
    {
        private static readonly int StateHash = Animator.StringToHash("state");

        public void OnUpdate(ref SystemState state)
        {
            var inputComponent = SystemAPI.GetSingleton<InputComponent>();
            foreach (var (animatorComponent, localTransform) in SystemAPI
                         .Query<RefRO<AnimatorComponent>, RefRO<LocalTransform>>())
            {
                var position = localTransform.ValueRO.Position;
                var rotation = localTransform.ValueRO.Rotation;
                animatorComponent.ValueRO.Ref.Value.transform.SetPositionAndRotation(
                    position,
                    rotation
                );
                animatorComponent.ValueRO.Ref.Value.SetInteger(StateHash, animatorComponent.ValueRO.State);
            }
        }
    }
}