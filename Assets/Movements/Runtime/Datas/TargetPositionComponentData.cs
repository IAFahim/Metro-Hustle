using Unity.Entities;
using Unity.Mathematics;

namespace Movements.Runtime.Datas
{
    public partial struct TargetPositionComponentData : IComponentData
    {
        public float3 Value;
    }
}