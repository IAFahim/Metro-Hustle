using _src.Scripts.Animations.Animations.Data;
using Unity.Entities;
using UnityEngine;
using UnityEngine.Search;

namespace _src.Scripts.Animations.Animations.Authoring
{
    public class AnimatorAuthoring : MonoBehaviour
    {
        [SearchContext("t:prefab")]
        public GameObject prefab;

        private class AnimatorAuthoringBaker : Baker<AnimatorAuthoring>
        {
            public override void Bake(AnimatorAuthoring authoring)
            {
                var entity = GetEntity(TransformUsageFlags.None);
                AddComponentObject(entity, new PrefabComponentClass()
                {
                    Prefab = authoring.prefab
                });
            }
        }
    }
}