#if UNITY_EDITOR
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _src.Scripts.Dimensions.Runtime.Datas
{
    public class Dimensions2DComponentDataAuthoring : MonoBehaviour
    {
        public float2 value = new(1, 1);

        public class AreaComponentDataBaker : Baker<Dimensions2DComponentDataAuthoring>
        {
            public override void Bake(Dimensions2DComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new Dimensions2DComponentData { Value = authoring.value });
            }
        }
    }
}

#endif