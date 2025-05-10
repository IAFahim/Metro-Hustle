using BovineLabs.Core.PhysicsStates;
using Unity.Entities;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    public float MaxHealth = 100f;

    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerTag());
            AddComponent(entity, new Health
            {
                Max = authoring.MaxHealth,
                Current = authoring.MaxHealth // Start at full health
            });
            // Player needs the buffer to *receive* trigger events
            AddBuffer<StatefulTriggerEvent>(entity);
        }
    }
}