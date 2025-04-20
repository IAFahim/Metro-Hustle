using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Bush_Scene_Component_add
{
    public class FloorsAddingAuthoring : MonoBehaviour
    {
        public GameObject[] floors;
        public class FloorsAddingBaker : Baker<FloorsAddingAuthoring>
        {
            public override void Bake(FloorsAddingAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                foreach (var floor in authoring.floors )
                {
                    AddComponent<FloorsAdding>(entity,new FloorsAdding()
                    {
                        Floor = GetEntity(floor, TransformUsageFlags.None)
                    });
                    
                }
               
            }
        }
    }
}