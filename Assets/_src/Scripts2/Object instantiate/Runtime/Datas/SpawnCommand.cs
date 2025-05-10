using BovineLabs.Core.ObjectManagement;
using Unity.Entities;

namespace _src.Scripts.Object_instantiate.Runtime.Datas
{
    public struct SpawnCommand : IComponentData
    {
        public ObjectId Prefab;
    }

}