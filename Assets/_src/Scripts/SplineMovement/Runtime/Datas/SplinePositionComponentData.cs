using Unity.Entities;
using Unity.Mathematics;

namespace _src.Scripts.SplineMovement.Runtime.Datas
{
    public struct SplinePositionComponentData : IComponentData
    {
        public float3 Position;
    }
}