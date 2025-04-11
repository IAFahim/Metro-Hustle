using Unity.Entities;

namespace _src.Scripts.SplineMovement.Runtime.Datas
{
    public struct SplineLinkComponentData : IComponentData
    {
        public ushort SplineIndex;
        public ushort KnotIndex;
        public float Progress;
    }
}