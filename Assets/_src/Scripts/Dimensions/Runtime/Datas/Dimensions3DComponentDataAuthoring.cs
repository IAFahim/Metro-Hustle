#if UNITY_EDITOR
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _src.Scripts.Dimensions.Runtime.Datas
{
    internal class Dimensions3DComponentDataAuthoring : MonoBehaviour
    {
        public float3 value = new(1, 1, 1);

        public class Dimensions3DComponentDataBaker : Baker<Dimensions3DComponentDataAuthoring>
        {
            public override void Bake(Dimensions3DComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new Dimensions3DComponentData { Value = authoring.value });
            }
        }
    }
}
#endif