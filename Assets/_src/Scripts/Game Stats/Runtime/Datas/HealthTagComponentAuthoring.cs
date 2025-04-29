using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Game_Stats.Runtime.Datas
{
    public class HealthTagComponentAuthoring : MonoBehaviour
    {
        public class HealthTagComponentBaker : Baker<HealthTagComponentAuthoring>
        {
            public override void Bake(HealthTagComponentAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<HealthTagComponent>(entity);
            }
        }
    }
}