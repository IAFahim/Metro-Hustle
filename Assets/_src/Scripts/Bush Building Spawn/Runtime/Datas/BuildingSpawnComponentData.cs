using Unity.Entities;

public struct BuildingSpawnComponentData : IComponentData
{
    public Entity FloorPrefab;
    public int Count;
    public int HighestCount;
}