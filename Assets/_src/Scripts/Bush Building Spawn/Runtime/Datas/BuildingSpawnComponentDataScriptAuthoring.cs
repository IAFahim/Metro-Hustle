using Unity.Entities;
using UnityEngine;

public class BuildingSpawnComponentDataScriptAuthoring : MonoBehaviour
{
    public GameObject[] floors;
    public class BuildingSpawnComponentDataScriptBaker : Baker<BuildingSpawnComponentDataScriptAuthoring>
    {
        public override void Bake(BuildingSpawnComponentDataScriptAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            foreach (var floorPrefab in authoring.floors)
            {
                AddComponent<BuildingSpawnComponentData>(entity, new BuildingSpawnComponentData()
                {
                    FloorPrefab = GetEntity(floorPrefab,TransformUsageFlags.None)
                });
                
            }

          
            
        }
    }
}