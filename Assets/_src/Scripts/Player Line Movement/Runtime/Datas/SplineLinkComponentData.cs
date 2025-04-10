using Unity.Entities;

namespace _src.Scripts.Player_Line_Movement.Runtime.Datas
{
    public struct SplineLinkComponentData : IComponentData
    {
        public ushort SplineIndex;
        public ushort KnotIndex;
        public float Progress;
    }
}