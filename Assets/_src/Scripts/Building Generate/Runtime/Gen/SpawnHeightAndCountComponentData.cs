using Unity.Entities;
using Unity.Mathematics;

namespace _src.Scripts.Building_Generate.Runtime.Gen
{
    public struct SpawnHeightAndCountComponentData : IComponentData, IEnableableComponent
    {
        public int Count;
    }
}