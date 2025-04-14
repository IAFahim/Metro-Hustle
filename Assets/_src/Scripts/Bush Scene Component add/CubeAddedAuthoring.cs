using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Bush_Scene_Component_add
{
    public class CubeAddedAuthoring : MonoBehaviour
    {
        public int cube;
        public class CubeAddedBaker : Baker<CubeAddedAuthoring>
        {
            public override void Bake(CubeAddedAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponent<CubeAdded>(entity,new CubeAdded()
                {
                    number = authoring.cube
                });
            }
        }
    }
}