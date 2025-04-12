using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    public class SpeedComponentDataAuthoring : MonoBehaviour
    {
        public float speed = 0.1f;
        public float multiplier = 1;

        public class SpeedComponentDataBaker : Baker<SpeedComponentDataAuthoring>
        {
            public override void Bake(SpeedComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SpeedComponentData
                {
                    BaseSpeed = authoring.speed,
                    Multiplier = authoring.multiplier
                });
            }
        }
    }
}