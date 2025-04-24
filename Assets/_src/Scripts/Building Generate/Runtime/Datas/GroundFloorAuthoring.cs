#if UNITY_EDITOR
using Unity.Entities;
using Unity.Transforms;
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
                var entity = GetEntity(TransformUsageFlags.ManualOverride);
                var bufferGroundFloor = AddBuffer<GroundFloorBuffer>(entity);
                foreach (var pair in authoring.prefabScalePair)
                {
                    bufferGroundFloor.Add(new GroundFloorBuffer()
                    {
                        Entity = GetEntity(pair.prefab, TransformUsageFlags.Renderable),
                        ScaleRange = 1 / pair.scaleRange
                    });
                }

                authoring.transform.GetPositionAndRotation(out Vector3 position, out var rotation);
                var scale = authoring.transform.localScale.z;
                AddComponent(entity, new LocalTransform()
                {
                    Position = position,
                    Rotation = rotation,
                    Scale = scale
                });
            }
        }
    }
}
#endif