using _src.Scripts.Main_Player_Maker.Runtime.Datas;
using _src.Scripts.Player_Line_Movement.Runtime.Datas;
using _src.Scripts.Speeds.Runtime.Datas;
using BovineLabs.Core.Input;
using ECS_Spline.Runtime.Datas;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace _src.Scripts.Player_Line_Movement.Runtime.Systems
{
    [BurstCompile]
    public partial struct PlayerLineMovementSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SplineBlobAssetComponent>();
            state.RequireForUpdate<InputCoreComponentData>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var inputMapComponentData = SystemAPI.GetSingleton<InputCoreComponentData>();
            var splineEntity = SystemAPI.GetSingletonEntity<SplineBlobAssetComponent>();
            var splineLocalTransform = SystemAPI.GetComponent<LocalTransform>(splineEntity);
            var splineBlobAssetComponent = SystemAPI.GetComponent<SplineBlobAssetComponent>(splineEntity);
            ref var knots = ref splineBlobAssetComponent.Reference.Value.knots;

            foreach (
                var (
                    localTransform,
                    speedComponentData,
                    splineLinkComponentData
                    )
                in SystemAPI.Query<
                    RefRW<LocalTransform>,
                    RefRO<SpeedComponentData>,
                    RefRW<SplineLinkComponentData>
                >().WithPresent<MainPlayerTagComponentData>())
            {
                var moveCurrentY = inputMapComponentData.MoveCurrent.y;
                if (moveCurrentY == 0) return;
                var isForward = moveCurrentY > 0;
                if (isForward)
                {
                    if (splineLinkComponentData.ValueRO.Progress >= 1)
                    {
                        splineLinkComponentData.ValueRW.Progress = 0;
                        splineLinkComponentData.ValueRW.KnotIndex++;
                    }

                    splineLinkComponentData.ValueRW.Progress += speedComponentData.ValueRO.GetCurrentSpeed();
                    Debug.Log(splineLinkComponentData.ValueRO.Progress+"");
                }
                
                var bezierKnot = knots[splineLinkComponentData.ValueRW.KnotIndex];
                var positionInSpline = bezierKnot.Position + splineLocalTransform.Position;
                localTransform.ValueRW.Position = positionInSpline;
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}