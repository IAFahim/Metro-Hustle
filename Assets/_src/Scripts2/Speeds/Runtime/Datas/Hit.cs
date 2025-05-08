using BovineLabs.Core.PhysicsStates; // For StatefulTriggerEvent & StatefulEventState
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Physics.Authoring;
using Unity.Physics.Systems;
using Unity.Transforms;

// --- Components ---

// Tag for player entities
public struct PlayerTag : IComponentData {}

// Tag for the trigger zone
public struct HealZoneTag : IComponentData {}

// Component for the player's health
public struct Health : IComponentData
{
    public float Current;
    public float Max;
}

// Component on the HealZone specifying healing properties
public struct HealOverTime : IComponentData
{
    public float HealPerSecond;
}

// --- Authoring ---

// Add this to your Player prefab/GameObject

// Add this to your Heal Zone prefab/GameObject
// REMEMBER TO SET 'Is Trigger' = true on the PhysicsShape authoring component!


// --- System ---

// Update after physics simulation and after stateful events are processed
[UpdateInGroup(typeof(PhysicsSystemGroup))]
[UpdateAfter(typeof(PhysicsSimulationGroup))]
[UpdateAfter(typeof(StatefulTriggerEventSystem))] // Ensure events are populated first
[BurstCompile]
public partial struct PlayerHealZoneSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // Ensure the system waits for necessary components if they aren't present at startup
        state.RequireForUpdate<PlayerTag>();
        state.RequireForUpdate<HealZoneTag>();
    }

    [BurstCompile]
    public void OnDestroy(ref SystemState state) { }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Dependency = new PlayerHealZoneJob
        {
            HealZoneLookup = SystemAPI.GetComponentLookup<HealZoneTag>(true),
            HealOverTimeLookup = SystemAPI.GetComponentLookup<HealOverTime>(true),
            DeltaTime = SystemAPI.Time.DeltaTime
            // Health component lookup will be handled by the IJobEntity query
        }.ScheduleParallel(state.Dependency);
    }

    // Use WithChangeFilter to only process players whose trigger buffer *changed* this frame
    [BurstCompile]
    [WithChangeFilter(typeof(StatefulTriggerEvent))]
    private partial struct PlayerHealZoneJob : IJobEntity
    {
        [ReadOnly] public ComponentLookup<HealZoneTag> HealZoneLookup;
        [ReadOnly] public ComponentLookup<HealOverTime> HealOverTimeLookup;
        public float DeltaTime;

        // PlayerTag and StatefulTriggerEvent buffer are required by the query
        // Health is read-write
        private void Execute(in PlayerTag player, ref Health health, in DynamicBuffer<StatefulTriggerEvent> triggerEvents)
        {
            bool isInHealZone = false;
            float healRate = 0f;

            for (int i = 0; i < triggerEvents.Length; i++)
            {
                var triggerEvent = triggerEvents[i];
                Entity otherEntity = triggerEvent.EntityB; // EntityB is the *other* entity in the event

                // Check if the other entity is a heal zone
                if (!HealZoneLookup.HasComponent(otherEntity))
                {
                    continue; // Not interacting with a heal zone in this specific event
                }

                // Process based on the state
                switch (triggerEvent.State)
                {
                    case StatefulEventState.Enter:
                        // Optional: Trigger particle effects, sound, UI notification for entering
                        //Debug.Log("Player entered heal zone");
                        isInHealZone = true; // Mark that we are definitely in *a* heal zone
                        // Get the heal rate from the specific zone entered
                        if (HealOverTimeLookup.TryGetComponent(otherEntity, out var healData))
                        {
                            healRate = math.max(healRate, healData.HealPerSecond); // Use the highest rate if overlapping zones
                        }
                        break;

                    case StatefulEventState.Stay:
                        // Apply healing over time
                        isInHealZone = true;
                        if (HealOverTimeLookup.TryGetComponent(otherEntity, out healData))
                        {
                             healRate = math.max(healRate, healData.HealPerSecond);
                        }
                        break;

                    case StatefulEventState.Exit:
                        // Optional: Stop particle effects, sound, etc. for exiting
                        //Debug.Log("Player exited heal zone");
                        // We don't set isInHealZone = false here, because the player might still be
                        // in *another* overlapping heal zone. We determine the final state below.
                        break;

                    // case StatefulEventState.Undefined: // Usually not needed for triggers
                    //     break;
                }
            }

            // After checking all events for this player this frame, apply healing if necessary
            if (isInHealZone && healRate > 0)
            {
                health.Current += healRate * DeltaTime;
                health.Current = math.min(health.Current, health.Max); // Clamp to max health
            }
        }
    }
}