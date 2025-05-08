#if UNITY_EDITOR
using BovineLabs.Core.Authoring.ObjectManagement;
using Unity.Entities;
using UnityEngine;

namespace _src.Scripts.Speeds.Runtime.Datas
{
    [RequireComponent(typeof(ObjectDefinitionAuthoring))]
    public class SpeedComponentDataAuthoring : MonoBehaviour
    {
        public float multiplier = 1f;
        

        public class SpeedComponentDataBaker : Baker<SpeedComponentDataAuthoring>
        {
            public override void Bake(SpeedComponentDataAuthoring authoring)
            {
                
                var entity = GetEntity(TransformUsageFlags.Dynamic);
                AddComponent(entity, new SpeedMultiplierComponentData
                {
                    Multiplier = authoring.multiplier
                });
            }
        }
        
    }
}
#endif