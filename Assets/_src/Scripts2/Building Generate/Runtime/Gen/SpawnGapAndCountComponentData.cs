using Unity.Entities;

namespace _src.Scripts.Building_Generate.Runtime.Gen
{
    public struct SpawnGapAndCountComponentData : IComponentData, IEnableableComponent
    {
        public float Gap;
        public int Count;
    }
}