using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Player_Line_Movement.Runtime.Datas
{
    public class SplineLinkComponentDataAuthoring : MonoBehaviour
    {
        public ushort splineIndex;
        public ushort knotIndex;
        public float progress;

        public class SplineLinkComponentDataBaker : Baker<SplineLinkComponentDataAuthoring>
        {
            public override void Bake(SplineLinkComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new SplineLinkComponentData
                    {
                        SplineIndex = authoring.splineIndex,
                        KnotIndex = authoring.knotIndex,
                        Progress = authoring.progress
                    }
                );
            }
        }
    }
}