using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Main_Player_Maker.Runtime.Datas
{
    public class MainPlayerTagComponentDataAuthoring : MonoBehaviour
    {
        public class MainPlayerTagComponentDataBaker : Baker<MainPlayerTagComponentDataAuthoring>
        {
            public override void Bake(MainPlayerTagComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<MainPlayerTagComponentData>(entity);
            }
        }
    }
}