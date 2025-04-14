using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    public class MiddleFloorAuthoring : MonoBehaviour
    {
        public GameObject middleFloorIndexes;
        public class MiddleFloorBaker : Baker<MiddleFloorAuthoring>
        {
            public override void Bake(MiddleFloorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var bufferMiddleFloor = AddBuffer<MiddleFloor>(entity);
                foreach (var authoringMiddleFloorIndex in authoring.middleFloorIndexes)
                {
                    bufferMiddleFloor.Add(new MiddleFloor()
                    {
                        MiddleFloorEntity = authoringMiddleFloorIndex
                    });
                }
            }
        }
    }
}