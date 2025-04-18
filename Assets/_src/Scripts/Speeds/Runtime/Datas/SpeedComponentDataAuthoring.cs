using Unity.Entities;
using UnityEngine;
using UnityEngine.Serialization;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    public class SpeedComponentDataAuthoring : MonoBehaviour
    {
        public float meterPerSecond = 1.3f;

        public class SpeedComponentDataBaker : Baker<SpeedComponentDataAuthoring>
        {
            public override void Bake(SpeedComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SpeedComponentData
                {
                    MeterPerSecond = authoring.meterPerSecond
                });
            }
        }
    }
}