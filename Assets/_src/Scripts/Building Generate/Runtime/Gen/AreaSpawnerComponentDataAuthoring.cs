using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Gen
{
    public class AreaSpawnerComponentDataAuthoring : MonoBehaviour
    {
        public float3 offset;

        public class AreaSpawnerComponentDataBaker : Baker<AreaSpawnerComponentDataAuthoring>
        {
            public override void Bake(AreaSpawnerComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new AreaSpawnerComponentData { Offset = authoring.offset });
            }
        }
    }
}