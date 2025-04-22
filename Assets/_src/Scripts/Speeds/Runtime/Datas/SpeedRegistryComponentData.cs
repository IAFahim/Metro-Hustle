using BovineLabs.Core.Iterators;
using BovineLabs.Core.ObjectManagement;
using Unity.Entities;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    [InternalBufferCapacity(0)]
    public struct SpeedRegistry : IDynamicHashMap<ObjectId, float>
    {
        public byte Value { get; }
    }
}