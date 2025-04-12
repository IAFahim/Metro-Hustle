using Unity.Entities;
using UnityEngine;

namespace SplineMovement.Runtime.Datas
{
    public class SplineLineNumberComponentDataAuthoring : MonoBehaviour
    {
        public sbyte value = 0;

        public class SplineOffsetComponentDataBaker : Baker<SplineLineNumberComponentDataAuthoring>
        {
            public override void Bake(SplineLineNumberComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SplineLineNumberComponentData
                {
                    Value = authoring.value
                });
            }
        }
    }
}