using Unity.Entities;
using UnityEngine;

namespace ECS_Spline.Runtime.Datas
{
    public class SplineMoverDataAuthoring : MonoBehaviour
    {
        public float Speed;

        public class SplineMoverDataBaker : Baker<SplineMoverDataAuthoring>
        {
            public override void Bake(SplineMoverDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SplineMoverData { speed = authoring.Speed });
            }
        }
    }
}