﻿#if UNITY_EDITOR
using BovineLabs.Core.Keys;
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Collision_Effect.Runtime.Datas
{
    public class CollisionEffectComponentDataAuthoring : MonoBehaviour
    {
        [K("CollisionEffectSettings", true)]
        public byte effectId;

        public class CollisionEffectComponentDataBaker : Baker<CollisionEffectComponentDataAuthoring>
        {
            public override void Bake(CollisionEffectComponentDataAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new CollisionEffectComponentData { EffectId = authoring.effectId });
            }
        }
    }
}
#endif