using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Search;

namespace _src.Scripts.Building_Generate.Runtime.Datas
{
    [Serializable]
    public class Building
    {
        [SearchContext("t:Prefab ground floor")]
        public GameObject prefab;
        public float3 size;
    }
}