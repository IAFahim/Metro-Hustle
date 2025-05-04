using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    public class SpeedComponentAuthoring : MonoBehaviour
    {
        public class SpeedComponentBaker : Baker<SpeedComponentAuthoring>
        {
            public override void Bake(SpeedComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<SpeedComponent>(entity);
            }
        }
    }
}