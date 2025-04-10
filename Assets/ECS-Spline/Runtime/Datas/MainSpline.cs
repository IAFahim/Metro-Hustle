using UnityEngine;
using UnityEngine.Splines;

namespace ECS_Spline.Runtime.Datas
{
    public class MainSpline : MonoBehaviour
    {
        public static SplineContainer Reference { get; private set; }

        private void OnEnable()
        {
            Reference = GetComponent<SplineContainer>();
        }
    }
}