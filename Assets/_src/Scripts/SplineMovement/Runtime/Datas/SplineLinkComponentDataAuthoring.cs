using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.SplineMovement.Runtime.Datas
{
    public class SplineLinkComponentDataAuthoring : MonoBehaviour
    {
        public byte splineIndex;
        public byte curveIndex;
        public sbyte lineNumber;
        public float progress;
        public float traveledDistance;


        public class SplineLinkComponentDataBaker : Baker<SplineLinkComponentDataAuthoring>
        {
            public override void Bake(SplineLinkComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity,
                    new SplineLinkComponentData
                    {
                        SplineIndex = authoring.splineIndex,
                        CurveIndex = authoring.curveIndex,
                        DistancePassedInCurve = authoring.progress,
                        TraveledDistance = authoring.traveledDistance,
                        LineNumber = authoring.lineNumber
                    }
                );

                AddComponent(entity, new SplineEntityTransformTargetComponentData());
            }
        }
    }
}