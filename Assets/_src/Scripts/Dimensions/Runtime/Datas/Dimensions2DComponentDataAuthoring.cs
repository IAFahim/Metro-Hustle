#if UNITY_EDITOR
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _src.Scripts.Dimensions.Runtime.Datas
{
    public class Dimensions2DComponentDataAuthoring : MonoBehaviour
    {
        public TransformUsageFlags transformUsageFlags = TransformUsageFlags.Dynamic;
        public float2 areaComponentData = new(10, 10);

        public class AreaComponentDataBaker : Baker<Dimensions2DComponentDataAuthoring>
        {
            public override void Bake(Dimensions2DComponentDataAuthoring authoring)
            {
                var entity = GetEntity(authoring.transformUsageFlags);
                AddComponent(entity, new Dimensions2DComponentData { Float2 = authoring.areaComponentData });
            }
        }
    }
}

#endif