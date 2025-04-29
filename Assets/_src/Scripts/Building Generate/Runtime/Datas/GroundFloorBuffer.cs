using BovineLabs.Core.ObjectManagement;
using Unity.Entities;
using Unity.Mathematics;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    [InternalBufferCapacity(10)]
    public struct GroundFloorBuffer : IBufferElementData
    {
        public Entity Entity;
        public float3 RealScale;
        public float3 ScaleScaler;
    }
}