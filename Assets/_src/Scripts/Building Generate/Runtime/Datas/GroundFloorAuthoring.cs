#if UNITY_EDITOR
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    internal class GroundFloorAuthoring : MonoBehaviour
    {
        public PrefabScalePair[] prefabScalePair;

        internal class GroundFloorBaker : Baker<GroundFloorAuthoring>
        {
            public override void Bake(GroundFloorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var bufferGroundFloor = AddBuffer<GroundFloorBuffer>(entity);
                foreach (var pair in authoring.prefabScalePair)
                {
                    bufferGroundFloor.Add(new GroundFloorBuffer()
                    {
                        Entity = GetEntity(pair.prefab, TransformUsageFlags.Renderable),
                        Scale = pair.scale
                    });
                }
            }
        }
    }
}
#endif