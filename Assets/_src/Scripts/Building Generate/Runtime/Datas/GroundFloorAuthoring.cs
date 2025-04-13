using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    public class GroundFloorAuthoring : MonoBehaviour
    {
        public int[] groundFloorIndexes;
        public class GroundFloorBaker : Baker<GroundFloorAuthoring>
        {
            public override void Bake(GroundFloorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var bufferGroundFloor = AddBuffer<GroundFloor>(entity);
                foreach (var index in authoring.groundFloorIndexes)
                {
                    bufferGroundFloor.Add(new GroundFloor()
                    {
                        GroundFloorEntity = index
                    });
                }
                
            }
        }
    }
}