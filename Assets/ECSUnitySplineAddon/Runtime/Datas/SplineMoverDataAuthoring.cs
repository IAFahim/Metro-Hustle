using Unity.Entities;
using UnityEngine;

namespace ECSUnitySplineAddon.Runtime.Datas
{
    public class SplineMoverDataAuthoring : MonoBehaviour
    {
        public float speed;

        public class SplineMoverDataBaker : Baker<SplineMoverDataAuthoring>
        {
            public override void Bake(SplineMoverDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SplineMoverData { speed = authoring.speed });
            }
        }
    }
}