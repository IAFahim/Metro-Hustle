using Unity.Entities;
using Unity.Mathematics;

namespace _src.Scripts.Spawn_Points.Runtime.Datas
{
    public struct SpawnPointBufferElement : IBufferElementData
    {
        public float3 Position;
        public quaternion Rotation;
    }
}
