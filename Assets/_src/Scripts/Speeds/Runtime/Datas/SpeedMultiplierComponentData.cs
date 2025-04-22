using Unity.Burst;
using Unity.Entities;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    [BurstCompile]
    public struct SpeedMultiplierComponentData : IComponentData
    {
        public float Multiplier;
    }
}