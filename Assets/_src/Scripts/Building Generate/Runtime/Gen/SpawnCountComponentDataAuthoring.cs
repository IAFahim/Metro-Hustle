using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Gen
{
    public class SpawnCountComponentDataAuthoring : MonoBehaviour
    {
        public int count=5;

        public class SpawnCountComponentDataBaker : Baker<SpawnCountComponentDataAuthoring>
        {
            public override void Bake(SpawnCountComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SpawnCountComponentData { Count = authoring.count });
            }
        }
    }
}