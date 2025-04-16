using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _src.Scripts.Areas.Runtime.Datas
{
    public class AreaComponentDataAuthoring : MonoBehaviour
    {
        public TransformUsageFlags transformUsageFlags = TransformUsageFlags.Dynamic;
        public float2 areaComponentData = new(10, 10);

        public class AreaComponentDataBaker : Baker<AreaComponentDataAuthoring>
        {
            public override void Bake(AreaComponentDataAuthoring authoring)
            {
                var entity = GetEntity(authoring.transformUsageFlags);
                AddComponent(entity, new AreaComponentData { Value = authoring.areaComponentData });
            }
        }
    }
}