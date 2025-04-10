using ECS_Spline.Runtime.Datas;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace ECS_Spline.Runtime.Systems
{
    public partial struct SplineLog : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<SplineBlobAssetComponent>();
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            var splineBlobAssetComponent = SystemAPI.GetSingleton<SplineBlobAssetComponent>();
            ref var splineBlob = ref splineBlobAssetComponent.Reference.Value;
            
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {

        }
    }
}