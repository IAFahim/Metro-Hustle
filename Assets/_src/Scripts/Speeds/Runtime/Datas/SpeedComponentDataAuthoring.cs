#if UNITY_EDITOR
using System;
using System.Linq;
using BovineLabs.Core.Authoring.ObjectManagement;
using BovineLabs.Core.Authoring.Settings;
using JetBrains.Annotations;
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    [RequireComponent(typeof(ObjectDefinitionAuthoring))]
    public class SpeedComponentDataAuthoring : LookupAuthoring<SpeedRegistry, float>
    {
        public float speed = 1;
        public float multiplier = 1f;
        [NonSerialized] [CanBeNull] private ObjectDefinitionAuthoring _objectDefinition;
        [NonSerialized] [CanBeNull] private SpeedSettings _speedSettings;

        public class SpeedComponentDataBaker : Baker<SpeedComponentDataAuthoring>
        {
            public override void Bake(SpeedComponentDataAuthoring authoring)
            {
                if (authoring._speedSettings == null && !authoring.TryGetInitialization(out float speed)) return;
                DependsOn(authoring._speedSettings);
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SpeedMultiplierComponentData
                {
                    Multiplier = authoring.multiplier
                });
            }
        }

        private void OnValidate()
        {
            _objectDefinition = GetComponent<ObjectDefinitionAuthoring>();
            TryGetInitialization(out var _);
        }


        public override bool TryGetInitialization(out float value)
        {
            value = speed;
            if (_speedSettings == null) _speedSettings = AuthoringSettingsUtility.GetSettings<SpeedSettings>();
            if (_speedSettings == null) return false;
            if (_objectDefinition == null) return false;
            if (_objectDefinition.Definition == null) return false;
            _speedSettings.Set(_objectDefinition.Definition, value);
            return true;
        }
    }
}
#endif