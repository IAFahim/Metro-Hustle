using Unity.Burst;
using Unity.Entities;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    [BurstCompile]
    public partial struct SpeedComponentData : IComponentData
    {
        public float MeterPerSecond;
    }
}