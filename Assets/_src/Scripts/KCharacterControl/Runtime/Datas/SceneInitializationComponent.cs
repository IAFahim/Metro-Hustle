using Unity.Entities;

namespace _src.Scripts.KCharacterControl.Runtime.Datas
{
    public struct SceneInitializationComponent : IComponentData
    {
        public Entity CharacterSpawnPointEntity;
        public Entity CharacterPrefabEntity;
        public Entity CameraPrefabEntity;
        public Entity PlayerPrefabEntity;
    }
}
