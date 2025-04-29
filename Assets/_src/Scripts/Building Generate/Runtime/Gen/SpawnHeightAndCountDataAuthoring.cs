#if UNITY_EDITOR
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Gen
{
    internal class SpawnHeightAndCountDataAuthoring : MonoBehaviour
    {
        public int count = 5;

        public class SpawnHeightAndCountComponentDataBaker : Baker<SpawnHeightAndCountDataAuthoring>
        {
            public override void Bake(SpawnHeightAndCountDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity,
                    new SpawnHeightAndCountComponentData
                    { 
                        Count = authoring.count,
                    });
            }
        }
    }
}
#endif