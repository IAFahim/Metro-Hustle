using Unity.Entities;
using UnityEngine;

public class BuildingSpawnComponentDataScriptAuthoring : MonoBehaviour
{
    public GameObject[] floors;
    public int count;
    public int highestCount;
    public class BuildingSpawnComponentDataScriptBaker : Baker<BuildingSpawnComponentDataScriptAuthoring>
    {
        public override void Bake(BuildingSpawnComponentDataScriptAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new BuildingSpawnComponentData()
                {
                    FloorPrefab = GetEntity(authoring.floors[0],TransformUsageFlags.None),
                    Count=authoring.count,
                    HighestCount =  authoring.highestCount
                   
                }
            );
        }
    }
}