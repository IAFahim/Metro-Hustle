using BovineLabs.Stats.Data;
using Unity.Burst;
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Speeds.Runtime.Systems
{
    public partial struct SpeedIncreseSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
            int count = 0;
            foreach (var buffer in SystemAPI.Query<DynamicBuffer<Intrinsic>>())
            {
                var intrinsics = buffer.AsMap();
                IntrinsicKey healthKey = new IntrinsicKey { Value = (ushort)0 };
                // Or use implicit conversion if available
                // IntrinsicKey healthKey = IntrinsicKeys.Health;
                
                var health = intrinsics.GetValue(healthKey);
                
                // Subtract damage from health
                count++;
                intrinsics.GetOrAddRef(healthKey) -= count;
            }

            Debug.Log(count);
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}