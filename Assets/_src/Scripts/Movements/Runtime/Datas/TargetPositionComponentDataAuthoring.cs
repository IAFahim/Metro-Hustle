using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _src.Scripts.Movements.Runtime.Datas
{
    public class TargetPositionComponentDataAuthoring : MonoBehaviour
    {
        public float3 TargetPositionComponentData;

        public class TargetPositionComponentDataBaker : Baker<TargetPositionComponentDataAuthoring>
        {
            public override void Bake(TargetPositionComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new TargetPositionComponentData { Value = authoring.TargetPositionComponentData });
            }
        }
    }
}