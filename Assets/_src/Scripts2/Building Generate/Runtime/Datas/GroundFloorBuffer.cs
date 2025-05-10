using Unity.Entities;
using Unity.Mathematics;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    [InternalBufferCapacity(16)]
    public struct GroundFloorBuffer : IBufferElementData
    {
        public Entity Prefab;
    }
}