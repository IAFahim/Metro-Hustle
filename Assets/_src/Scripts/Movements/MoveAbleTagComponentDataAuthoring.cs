using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Movements
{
    public class MoveAbleTagComponentDataAuthoring : MonoBehaviour
    {
        public class MoveAbleTagComponentDataBaker : Baker<MoveAbleTagComponentDataAuthoring>
        {
            public override void Bake(MoveAbleTagComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent<MoveAbleTagComponentData>(entity);
            }
        }
    }
}