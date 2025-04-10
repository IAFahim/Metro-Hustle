using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace ECS_Spline.Runtime.Datas
{
    [RequireComponent(typeof(SplineContainer))]
    public class SplineComponentAuthoring : MonoBehaviour
    {
        public class SplineComponentBaker : Baker<SplineComponentAuthoring>
        {
            public override void Bake(SplineComponentAuthoring authoring)
            {
                var splineContainer = GetComponent<SplineContainer>();

                if (splineContainer is null)
                {
                    Debug.Log($"From {nameof(SplineComponentBaker)}.Bake(). spline container is null");
                    return;
                }

                var spline = splineContainer.Spline;
                using var nativeSpline = new NativeSpline(spline, Allocator.Temp);

                var nativeSplineBlobAssetRef = NativeSplineBlob.CreateNativeSplineBlobAssetRef(
                    nativeSpline,
                    spline.Closed);

                var entity = GetEntity(TransformUsageFlags.Dynamic);

                AddBlobAsset(ref nativeSplineBlobAssetRef, out _);

                AddComponent(entity, new SplineBlobAssetComponent
                {
                    Reference = nativeSplineBlobAssetRef
                });
            }
        }
    }
}