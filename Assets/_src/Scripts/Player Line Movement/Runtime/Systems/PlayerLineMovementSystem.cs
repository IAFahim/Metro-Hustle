using BovineLabs.Core.Input;
using Unity.Burst;
using Unity.Entities;

namespace _src.Scripts.Player_Line_Movement.Runtime.Systems
{
    [BurstCompile]
    public partial struct PlayerLineMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<InputCoreComponentData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var inputMapComponentData = SystemAPI.GetSingleton<InputCoreComponentData>();
            // Debug.Log(inputMapComponentData.MoveCurrent+ " ");
            // foreach (var (localTransform, speedComponentData)
            //          in SystemAPI.Query<RefRW<LocalTransform>, RefRO<SpeedComponentData>>()
            //         )
            // {
            //     localTransform.ValueRW.Position.xz +=
            //         inputMapComponentData.MoveCurrent.xy * speedComponentData.ValueRO.Speed;
            // }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}