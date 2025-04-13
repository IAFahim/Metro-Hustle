using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    public class GroundFloorAuthoring : MonoBehaviour
    {
        public Building[] groundFloors;

        public class GroundFloorBaker : Baker<GroundFloorAuthoring>
        {
            public override void Bake(GroundFloorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var bufferGroundFloor = AddBuffer<GroundFloor>(entity);
                foreach (var building in authoring.groundFloors)
                {
                    bufferGroundFloor.Add(new GroundFloor()
                    {
                        Prefab = GetEntity(building.prefab, TransformUsageFlags.Renderable)
                    });
                }
            }
        }
    }
}