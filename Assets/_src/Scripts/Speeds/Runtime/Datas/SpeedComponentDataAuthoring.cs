using Unity.Entities;
using UnityEngine;

namespace Speeds.Runtime.Datas
{
    public class SpeedComponentDataAuthoring : MonoBehaviour
    {
        public float speed = 0.001f;

        public class SpeedComponentDataBaker : Baker<SpeedComponentDataAuthoring>
        {
            public override void Bake(SpeedComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SpeedComponentData { Speed = authoring.speed });
            }
        }
    }
}