#if UNITY_EDITOR
using _src.Scripts.Building_Generate.Runtime.Gen;
using _src.Scripts.Dimensions.Runtime.Datas;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    [RequireComponent(typeof(Dimensions2DComponentDataAuthoring))]
    [RequireComponent(typeof(SpawnHeightAndCountDataAuthoring))]
    internal class GroundFloorAuthoring : MonoBehaviour
    {
        public PrefabScalePair[] prefabScalePair;

        private void OnDrawGizmosSelected()
        {
            var position = transform.position;
            var length = prefabScalePair.Length;
            for (var i = 0; i < length; i++)
            {
                var pair = prefabScalePair[i];
                var alpha = (i + 1f) / length;
                DrawBottomOffset(position, pair.realScale, Color.red, alpha);
                DrawBottomOffset(position, pair.realScale * pair.scaleScaler, Color.cyan, alpha);
            }
        }

        private static void DrawBottomOffset(Vector3 position, float3 scale, Color color, float alpha)
        {
            var yOffsetPositionC0 = new Vector3(position.x, position.y + (scale.y / 2), position.z);
            Gizmos.color = new Color(color.r, color.g, color.b, alpha);
            Gizmos.DrawWireCube(yOffsetPositionC0, scale);
        }


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
                        RealScale = pair.realScale,
                        ScaleScaler = pair.scaleScaler
                    });
                }

                authoring.transform.GetPositionAndRotation(out Vector3 position, out var rotation);
                var scale = authoring.transform.localScale.y;
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