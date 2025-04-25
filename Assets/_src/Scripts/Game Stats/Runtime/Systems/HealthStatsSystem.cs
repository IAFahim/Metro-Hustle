using _src.Scripts.Game_Stats.Runtime.Datas;
using BovineLabs.Reaction.Data.Active;
using BovineLabs.Stats.Data;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace _src.Scripts.Game_Stats.Runtime.Systems
{
    public partial struct HealthStatsSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            added = false;
        }

        private bool added;

        public void OnUpdate(ref SystemState state)
        {
            foreach (var (_, entity) in SystemAPI.Query<RefRO<ActiveDuration>>().WithEntityAccess())
            {
                var stats = state.EntityManager.GetBuffer<Stat>(entity);
                var value = stats.GetValue(0);
                Debug.Log(value);
            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
            added = false;
        }
    }
}