using Unity.Entities;
using Unity.Mathematics;

namespace _src.Scripts.Building_Generate.Runtime.Gen
{
    public struct AreaSpawnerComponentData : IComponentData
    {
        public float2 Area;
    }
}