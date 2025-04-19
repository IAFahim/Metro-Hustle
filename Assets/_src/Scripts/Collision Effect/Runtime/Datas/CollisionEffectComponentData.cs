using BovineLabs.Core.Keys;
using Unity.Entities;

namespace _src.Scripts.Collision_Effect.Runtime.Datas
{
    public struct CollisionEffectComponentData : IComponentData
    {
        [K("CollisionEffectSettings ")]
        public int EffectId;
    }
}
