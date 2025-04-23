using System;
using _src.Scripts.Util.KeyValuePair.Runtime;
using BovineLabs.Core.Authoring.Settings;
using BovineLabs.Core.Settings;
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    [ResourceSettings("Speed")]
    public class SpeedSettings : SettingsBase
    {
        public KeyValuePair<int, float>[] keys = Array.Empty<KeyValuePair<int, float>>();

        public override void Bake(Baker<SettingsAuthoring> baker)
        {
            foreach (var kvPair in keys)
            {
                Debug.Log(kvPair);
            }
        }

        public void Set(int id, float value)
        {
            var index = Array.FindIndex(keys, kvPair => kvPair.key == id);
            if (-1 < index)
            {
                keys[index].value = value;
                return;
            }

            Array.Resize(ref keys, 1);
            keys[^1] = new KeyValuePair<int, float>()
            {
                key = id,
                value = value
            };
        }
    }
}