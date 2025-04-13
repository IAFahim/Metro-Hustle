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
        public void OnDestroy(ref SystemState state) { }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            float deltaTime = SystemAPI.Time.DeltaTime;

            Entity splineEntity = SystemAPI.GetSingletonEntity<NativeSplineBlobComponentData>();
            NativeSplineBlobComponentData splineComp = SystemAPI.GetComponent<NativeSplineBlobComponentData>(splineEntity);

            if (!splineComp.Value.IsCreated) return;

            ref var splineBlob = ref splineComp.Value.Value;

            LocalToWorld splineTransform = default;
            bool useTransform = SystemAPI.HasComponent<LocalTransform>(splineEntity);
             if (useTransform) {
                 splineTransform = SystemAPI.GetComponent<LocalToWorld>(splineEntity);
             }


            foreach (var (localTransform, splineLink, speedComponentData) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<SplineLinkComponentData>, RefRO<SpeedComponentData>>())
            {
                float speed = speedComponentData.ValueRO.GetCurrentSpeed() * deltaTime;
                float length = splineBlob.GetCurveLength(splineLink.ValueRO.CurveIndex);
                float distance = (splineLink.ValueRO.Progress) % length;
                float normalizedT = length > 0 ? distance / length : 0f;

                splineBlob.Evaluate(splineLink.ValueRO.CurveIndex,normalizedT, out float3 localPosition, out float3 localTangent, out float3 localUpVector);

                float3 worldPosition = localPosition;
                float3 worldTangent = localTangent;
                float3 worldUpVector = localUpVector;

                 if (useTransform)
                 {
                    worldPosition = math.transform(splineTransform.Value, localPosition);
                    worldTangent = math.rotate(splineTransform.Value, localTangent);
                    worldUpVector = math.rotate(splineTransform.Value, localUpVector);
                 }

                 localTransform.ValueRW.Position = worldPosition;
                if (math.lengthsq(worldTangent) > 0.0001f)
                {
                    localTransform.ValueRW.Rotation = quaternion.LookRotationSafe(math.normalize(worldTangent), math.normalize(worldUpVector));
                }

                splineLink.ValueRW.Progress += .001f;
            }
        }
    }
}