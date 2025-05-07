using BovineLabs.Core.PhysicsStates;
using Unity.Entities;
using UnityEngine;

public class HealZoneAuthoring : MonoBehaviour
{
    public float HealPerSecond = 5f;

    class Baker : Baker<HealZoneAuthoring>
    {
        public override void Bake(HealZoneAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None); // Static trigger likely doesn't need transform flags
            AddComponent(entity, new HealZoneTag());
            AddComponent(entity, new HealOverTime { HealPerSecond = authoring.HealPerSecond });
            // The zone *also* needs the buffer for the physics system to generate events involving it
            AddBuffer<StatefulTriggerEvent>(entity);
        }
    }
}