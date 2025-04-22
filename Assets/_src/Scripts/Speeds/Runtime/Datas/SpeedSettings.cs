using System;
using _src.Scripts.Util.KeyValuePair.Runtime;
using BovineLabs.Core.Authoring.ObjectManagement;
using BovineLabs.Core.Authoring.Settings;
using BovineLabs.Core.Settings;
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    [ResourceSettings("Speed")]
    public class SpeedSettings : SettingsBase
    {
        public KeyValuePair<ObjectDefinition, float>[] keys = Array.Empty<KeyValuePair<ObjectDefinition, float>>();

        public override void Bake(Baker<SettingsAuthoring> baker)
        {
            foreach (var kvPair in keys)
            {
                Debug.Log(kvPair);
            }
        }

        public void Set(ObjectDefinition definition, float value)
        {
            var index = Array.FindIndex(keys, kvPair => kvPair.key == definition);
            if (-1 < index)
            {
                keys[index].value = value;
                return;
            }

            Array.Resize(ref keys, 1);
            keys[^1] = new KeyValuePair<ObjectDefinition, float>()
            {
                key = definition,
                value = value
            };
        }
    }
}