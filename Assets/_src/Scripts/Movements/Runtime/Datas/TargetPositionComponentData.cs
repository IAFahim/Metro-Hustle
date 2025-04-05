using Unity.Entities;
using Unity.Mathematics;

namespace _src.Scripts.Movements.Runtime.Datas
{
    public partial struct TargetPositionComponentData : IComponentData
    {
        public float3 Value;
    }
}