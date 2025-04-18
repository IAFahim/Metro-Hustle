using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Players.Runtime.Datas
{
    public class PlayerComponentDataAuthoring : MonoBehaviour
    {
        public class PlayerComponentDataBaker : Baker<PlayerComponentDataAuthoring>
        {
            public override void Bake(PlayerComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<PlayerComponentData>(entity);
            }
        }
    }
}