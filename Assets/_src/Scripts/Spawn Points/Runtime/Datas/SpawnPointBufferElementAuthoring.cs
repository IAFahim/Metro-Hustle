using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Spawn_Points.Runtime.Datas
{
    public class SpawnPointBufferElementAuthoring : MonoBehaviour
    {
        public class SpawnPointBufferElementBaker : Baker<SpawnPointBufferElementAuthoring>
        {
            public override void Bake(SpawnPointBufferElementAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddBuffer<SpawnPointBufferElement>(entity);
            }
        }
    }
}