using Unity.Entities;
using UnityEngine;

public class AreaComponentDataScriptAuthoring : MonoBehaviour
{
    public float length = 100;
    public float width = 80;
    public class AreaComponentDataScriptBaker : Baker<AreaComponentDataScriptAuthoring>
    {
        public override void Bake(AreaComponentDataScriptAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent<AreaComponentDataScript>(entity,new AreaComponentDataScript()
            {
                Length = authoring.length,
                Width = authoring.width
            });
        }
    }
}