using BovineLabs.Core;
using BovineLabs.Core.Extensions;
using BovineLabs.Stats;
using BovineLabs.Stats.Data;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Speeds.Runtime.Systems
{
    public partial struct SpeedIncreseSystem : ISystem
    {
        private IntrinsicWriter.Lookup _intrinsicWriterLookup;
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
            this._intrinsicWriterLookup = new IntrinsicWriter.Lookup();
            this._intrinsicWriterLookup.Create(ref state); 
        }

        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            _intrinsicWriterLookup.Update(ref state);
            BLDebug debug = state.EntityManager.GetSingleton<BLDebug>(false);
            foreach (var (buffer, entity) in SystemAPI.Query<DynamicBuffer<Intrinsic>>().WithEntityAccess())
            {
                
                var intrinsics = buffer.AsMap();
                IntrinsicKey healthKey = new IntrinsicKey { Value = (ushort)0 };
                var health = intrinsics.GetValue(healthKey);
                IntrinsicWriter intrinsicWriter = _intrinsicWriterLookup[entity];
                
                var newHealth = _intrinsicWriterLookup[entity].Add(healthKey, 1);

            }
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}