using BovineLabs.Core.Authoring.ObjectManagement;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _src.Scripts.Object_instantiate.Runtime.Datas
{
    public class SpawnCommandAuthoring : MonoBehaviour
    {
        public ObjectDefinition prefab;

        public class Baker : Baker<SpawnCommandAuthoring>
        {
            public override void Bake(SpawnCommandAuthoring authoring)
            {
                AddComponent(GetEntity(TransformUsageFlags.None),
                    new SpawnCommand
                    {
                        Prefab = authoring.prefab,
                    }
                );
            }
        }
    }
}