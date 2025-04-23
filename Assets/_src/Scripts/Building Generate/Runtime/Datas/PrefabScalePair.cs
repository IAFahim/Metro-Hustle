#if UNITY_EDITOR
using System;
using BovineLabs.Core.Authoring.ObjectManagement;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Search;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    [Serializable]
    public class PrefabScalePair
    {
        [SearchContext("t:prefab gf")] public GameObject prefab;

        public float3 scale;
    }
}
#endif