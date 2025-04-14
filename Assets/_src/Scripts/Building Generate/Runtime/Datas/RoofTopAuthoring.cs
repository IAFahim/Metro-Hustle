using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    public class RoofTopAuthoring : MonoBehaviour
    {
        public GameObject[] roofTopIndexes;
        public class RoofTopBaker : Baker<RoofTopAuthoring>
        {
            public override void Bake(RoofTopAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                var bufferRoofTop = AddBuffer<RoofTop>(entity);
                foreach (var authoringRoofTopIndex in authoring.roofTopIndexes)
                {
                    bufferRoofTop.Add(new RoofTop()
                    {
                        RoofTopEntity = authoringRoofTopIndex
                    });
                }
            }
        }
    }
}