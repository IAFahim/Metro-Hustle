using System;
using System.Collections.Generic;
using System.Linq;
using BovineLabs.Core.Keys;
using UnityEngine;

namespace _src.Scripts.Collision_Effect.Runtime.Datas
{
    public class CollisionEffectSettings : KSettingsBase<CollisionEffectSettings, int>
    {
        [SerializeField] private KeyValues[] keys = Array.Empty<KeyValues>();

        // Implemented method to convert our custom authoring
        public override IEnumerable<NameValue<int>> Keys => keys.Select(k => new NameValue<int>(k.name, k.value));

        [Serializable]
        public class KeyValues
        {
            public string name = string.Empty;

            [Min(0)] public int value = -1;
        }
    }
}