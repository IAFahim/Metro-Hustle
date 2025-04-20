using System;
using System.Collections.Generic;
using System.Linq;
using BovineLabs.Core.Keys;
using UnityEngine;

namespace _src.Scripts.Collision_Effect.Runtime.Datas
{
    public class CollisionEffectSettings : KSettingsBase<CollisionEffectSettings, byte>
    {
        [SerializeField] private NameValue<byte>[] keys = Array.Empty<NameValue<byte>>();
        public override IEnumerable<NameValue<byte>> Keys => keys.Select(k => new NameValue<byte>(k.Name, k.Value));
        
    }
}