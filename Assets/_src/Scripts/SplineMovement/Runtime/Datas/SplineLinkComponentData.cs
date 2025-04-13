using Unity.Entities;

namespace _src.Scripts.SplineMovement.Runtime.Datas
{
    public struct SplineLinkComponentData : IComponentData
    {
        public byte SplineIndex;
        public byte CurveIndex;
        public sbyte LineNumber;
        public float DistancePassedInCurve;
        public float TraveledDistance;
    }
}