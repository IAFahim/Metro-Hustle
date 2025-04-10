using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine.Splines;

namespace ECS_Spline.Runtime.Datas
{
    public struct NativeSplineBlob
    {
        public BlobArray<BezierKnot> Knots;
        public BlobArray<BezierCurve> Curves;
        public BlobArray<DistanceToInterpolation> SegmentLengthsLookupTable;
        public BlobArray<float3> UpVectorsLookupTable;
        public bool Closed;
        public float Length;
        public const int SegmentResolution = 30;

        public static BlobAssetReference<NativeSplineBlob> CreateNativeSplineBlobAssetRef
        (
            NativeSpline nativeSpline,
            bool isClosed
        )
        {
            // Constructing blob
            using var nativeSplineBuilder = new BlobBuilder(Allocator.Temp);
            ref var nativeSplineRoot = ref nativeSplineBuilder.ConstructRoot<NativeSplineBlob>();
            
            StoreKnots(nativeSplineBuilder, ref nativeSplineRoot, nativeSpline.Knots);
            StoreCurves(nativeSplineBuilder, ref nativeSplineRoot, nativeSpline.Curves);
            nativeSplineRoot.Closed = isClosed;

            return nativeSplineBuilder
                .CreateBlobAssetReference<NativeSplineBlob>(Allocator.Persistent);
        }

        private static void StoreCurves(BlobBuilder nativeSplineBuilder, ref NativeSplineBlob nativeSplineRoot, NativeArray<BezierCurve> curves)
        {
            var length = curves.Length;
            var curveBuilder = nativeSplineBuilder.Allocate(ref nativeSplineRoot.Curves, length);
            for (int i = 0; i < length; i++) curveBuilder[i] = curves[i];
        }

        private static void StoreKnots(BlobBuilder nativeSplineBuilder, ref NativeSplineBlob nativeSplineRoot,
            NativeArray<BezierKnot> knots)
        {
            var length = knots.Length;
            var knotsBuilder = nativeSplineBuilder.Allocate(ref nativeSplineRoot.Knots, length);
            for (int i = 0; i < length; i++) knotsBuilder[i] = knots[i];
        }
    }
}