using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.KCharacterControl.Runtime.Datas
{
    public class SceneInitializationAuthoring : MonoBehaviour
    {
        public GameObject characterSpawnPointEntity;
        public GameObject characterPrefabEntity;
        public GameObject cameraPrefabEntity;
        public GameObject playerPrefabEntity;

        public class Baker : Baker<SceneInitializationAuthoring>
        {       
            public override void Bake(SceneInitializationAuthoring authoring)
            {
                Entity entity = GetEntity(TransformUsageFlags.None);
                AddComponent(entity, new SceneInitializationComponent
                {
                    CharacterSpawnPointEntity = GetEntity(authoring.characterSpawnPointEntity, TransformUsageFlags.Dynamic),
                    CharacterPrefabEntity = GetEntity(authoring.characterPrefabEntity, TransformUsageFlags.Dynamic),
                    CameraPrefabEntity = GetEntity(authoring.cameraPrefabEntity, TransformUsageFlags.Dynamic),
                    PlayerPrefabEntity = GetEntity(authoring.playerPrefabEntity, TransformUsageFlags.None),
                });
            }
        }
    }
}