using ECS_Spline.Runtime.Datas;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

namespace ECSUnitySplineAddon.Runtime.Datas
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
            float time = (float)SystemAPI.Time.ElapsedTime;

            Entity splineEntity = SystemAPI.GetSingletonEntity<NativeSplineBlobComponentData>();
            NativeSplineBlobComponentData splineComp = SystemAPI.GetComponent<NativeSplineBlobComponentData>(splineEntity);

            if (!splineComp.Value.IsCreated) return;

            ref var splineBlob = ref splineComp.Value.Value;

            LocalToWorld splineTransform = default;
            bool useTransform = SystemAPI.HasComponent<LocalToWorld>(splineEntity);
             if (useTransform) {
                 splineTransform = SystemAPI.GetComponent<LocalToWorld>(splineEntity);
             }


            foreach (var (transform, mover) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<SplineMoverData>>())
            {
                float speed = mover.ValueRO.speed;
                float length = splineBlob.Length;
                float distance = (time * speed) % length;
                float normalizedT = length > 0 ? distance / length : 0f;

                splineBlob.Evaluate(normalizedT, out float3 localPosition, out float3 localTangent, out float3 localUpVector);

                float3 worldPosition = localPosition;
                float3 worldTangent = localTangent;
                float3 worldUpVector = localUpVector;

                 if (useTransform)
                 {
                    worldPosition = math.transform(splineTransform.Value, localPosition);
                    worldTangent = math.rotate(splineTransform.Value, localTangent);
                    worldUpVector = math.rotate(splineTransform.Value, localUpVector);
                 }

                 transform.ValueRW.Position = worldPosition;
                if (math.lengthsq(worldTangent) > 0.0001f)
                {
                    transform.ValueRW.Rotation = quaternion.LookRotationSafe(math.normalize(worldTangent), math.normalize(worldUpVector));
                }
            }
        }
    }

    public struct SplineMoverData : IComponentData
    {
        public float speed;
    }
}