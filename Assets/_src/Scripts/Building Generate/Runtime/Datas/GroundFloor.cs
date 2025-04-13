using Unity.Entities;
using Unity.Mathematics;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    [InternalBufferCapacity(16)]
    public struct GroundFloor : IBufferElementData
    {
        public Entity Prefab;
        public float3 Size;
    }
}