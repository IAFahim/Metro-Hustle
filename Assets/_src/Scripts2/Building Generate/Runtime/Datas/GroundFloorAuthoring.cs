using Unity.Entities;
using UnityEngine;
using UnityEngine.Search;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    public class GroundFloorAuthoring : MonoBehaviour
    {
        [SearchContext("t:Prefab ground floor")]
        public GameObject[] groundFloors;

        public class GroundFloorBaker : Baker<GroundFloorAuthoring>
        {
            public override void Bake(GroundFloorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var bufferGroundFloor = AddBuffer<GroundFloorBuffer>(entity);
                foreach (var prefab in authoring.groundFloors)
                {
                    bufferGroundFloor.Add(new GroundFloorBuffer()
                    {
                        Prefab = GetEntity(prefab, TransformUsageFlags.Renderable)
                    });
                }
            }
        }
    }
}