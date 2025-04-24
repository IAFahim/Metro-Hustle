#if UNITY_EDITOR
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Search;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    [Serializable]
    public class PrefabScalePair
    {
        [SearchContext("t:prefab gf")] public GameObject prefab;

        public float3x2 scaleRange = new()
        {
            c0 = new float3(1, 1, 1),
            c1 = new float3(1.1f, 1.1f, 1.1f)
        };
    }
}
#endif