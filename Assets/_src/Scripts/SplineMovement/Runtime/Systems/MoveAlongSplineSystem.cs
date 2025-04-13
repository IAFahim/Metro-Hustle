using _src.Scripts.Speeds.Runtime.Datas;
using _src.Scripts.SplineMovement.Runtime.Datas;
using ECSUnitySplineAddon.Runtime.Datas;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace _src.Scripts.SplineMovement.Runtime.Systems
{
    [BurstCompile]
    public partial struct MoveAlongSplineSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            state.RequireForUpdate<NativeSplineBlobComponentData>();
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            Entity splineEntity = SystemAPI.GetSingletonEntity<NativeSplineBlobComponentData>();
            NativeSplineBlobComponentData splineComp =
                SystemAPI.GetComponent<NativeSplineBlobComponentData>(splineEntity);

            if (!splineComp.Value.IsCreated) return;

            ref var splineBlob = ref splineComp.Value.Value;

            LocalToWorld splineTransform = default;
            splineTransform = SystemAPI.GetComponent<LocalToWorld>(splineEntity);


            foreach (var (localTransform, splineLink, speedComponentData) in SystemAPI
                         .Query<RefRW<LocalTransform>, RefRW<SplineLinkComponentData>, RefRO<SpeedComponentData>>())
            {
                float curveLength = splineBlob.GetCurveLength(splineLink.ValueRO.CurveIndex);
                float offsetThisFrame = speedComponentData.ValueRO.GetCurrentSpeed() * deltaTime;
                splineLink.ValueRW.DistancePassedInCurve += offsetThisFrame;
                float normalizedT = splineLink.ValueRO.DistancePassedInCurve / curveLength;

                splineBlob.Evaluate(splineLink.ValueRO.CurveIndex, normalizedT, out float3 localPosition, out float3 localTangent, out float3 localUpVector);

                float3 worldPosition = math.transform(splineTransform.Value, localPosition);
                float3 worldTangent = math.rotate(splineTransform.Value, localTangent);
                float3 worldUpVector = math.rotate(splineTransform.Value, localUpVector);
                
                localTransform.ValueRW.Position = worldPosition;
                if (math.lengthsq(worldTangent) < float.Epsilon) continue;
                var forward = math.normalize(worldTangent);
                var up = math.normalize(worldUpVector);
                localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(forward, up);
            }
        }
    }
}