using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Building_Gen.Runtime.Datas
{
    public class GroundFloorsAuthoring : MonoBehaviour
    {
        public int[] groundFloorIndexes;

        public class GroundFloorsBaker : Baker<GroundFloorsAuthoring>
        {
            public override void Bake(GroundFloorsAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                var groundFloorIndexBuffer = AddBuffer<GroundFloors>(entity);
                foreach (var index in authoring.groundFloorIndexes)
                {
                    groundFloorIndexBuffer.Add(new GroundFloors()
                    {
                        Index = index
                    });
                }
            }
        }
    }
}