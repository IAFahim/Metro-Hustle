using Unity.Burst;
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
            float4x4 transform,
            bool cacheUpVectors
        )
        {
            using var nativeSplineBuilder = new BlobBuilder(Allocator.Temp);
            ref var nativeSplineRoot = ref nativeSplineBuilder.ConstructRoot<NativeSplineBlob>();
            NativeSplines(
                nativeSplineBuilder,
                nativeSpline,
                ref nativeSplineRoot,
                transform,
                cacheUpVectors
            );
            return nativeSplineBuilder.CreateBlobAssetReference<NativeSplineBlob>(Allocator.Persistent);
        }


        public static void NativeSplines(BlobBuilder blobbuilder, NativeSpline nativeSpline,
            ref NativeSplineBlob nativeSplineRoot, float4x4 transform, bool cacheUpVectors)
        {
            StoreKnots(ref nativeSplineRoot, blobbuilder, nativeSpline.Knots);
            StoreCurves(ref nativeSplineRoot, blobbuilder, nativeSpline.Curves);
            int knotCount = nativeSpline.Knots.Length;

            var segmentLengthsLookupTable = blobbuilder.Allocate(
                ref nativeSplineRoot.SegmentLengthsLookupTable,
                knotCount * SegmentResolution
            );
            nativeSplineRoot.Closed = nativeSpline.Closed;
            var length = 0.0f;

            var upVectorsLookupTable = blobbuilder.Allocate(ref nativeSplineRoot.UpVectorsLookupTable,
                cacheUpVectors ? knotCount * SegmentResolution : 0);

            var distanceToTimes = new NativeArray<DistanceToInterpolation>(SegmentResolution, Allocator.Temp);
            var upVectors = cacheUpVectors ? new NativeArray<float3>(SegmentResolution, Allocator.Temp) : default;

            if (knotCount > 0)
            {
                BezierKnot cur = nativeSpline.Knots[0].Transform(transform);
                for (int i = 0; i < knotCount; ++i)
                {
                    BezierKnot next = nativeSpline.Knots[(i + 1) % knotCount].Transform(transform);

                    CurveUtility.CalculateCurveLengths(nativeSpline.Curves[i], distanceToTimes);

                    if (cacheUpVectors)
                    {
                        var curveStartUp = math.rotate(cur.Rotation, math.up());
                        var curveEndUp = math.rotate(next.Rotation, math.up());
                        CurveUtilityInternal.EvaluateUpVectors(nativeSpline.Curves[i], curveStartUp, curveEndUp, upVectors);
                    }

                    if (nativeSpline.Closed || i < knotCount - 1)
                        length += distanceToTimes[SegmentResolution - 1].Distance;

                    for (int index = 0; index < SegmentResolution; index++)
                    {
                        segmentLengthsLookupTable[i * SegmentResolution + index] = distanceToTimes[index];

                        if (cacheUpVectors)
                            upVectorsLookupTable[i * SegmentResolution + index] = upVectors[index];
                    }

                    cur = next;
                }

                nativeSplineRoot.Length = length;
            }
        }

        private static void StoreCurves(ref NativeSplineBlob nativeSplineRoot,
            BlobBuilder blobBuilder,
            NativeArray<BezierCurve> curves)
        {
            var length = curves.Length;
            var curveBuilder = blobBuilder.Allocate(ref nativeSplineRoot.Curves, length);
            for (int i = 0; i < length; i++) curveBuilder[i] = curves[i];
        }

        private static void StoreKnots(ref NativeSplineBlob nativeSplineRoot,
            BlobBuilder blobBuilder,
            NativeArray<BezierKnot> knots
        )
        {
            var length = knots.Length;
            var knotsBuilder = blobBuilder.Allocate(ref nativeSplineRoot.Knots, length);
            for (int i = 0; i < length; i++) knotsBuilder[i] = knots[i];
        }
        
        /// <summary>
        /// Get a <see cref="BezierCurve"/> from a knot index.
        /// </summary>
        /// <param name="index">The knot index that serves as the first control point for this curve.</param>
        /// <returns>
        /// A <see cref="BezierCurve"/> formed by the knot at index and the next knot.
        /// </returns>
        public BezierCurve GetCurve(int index) => Curves[index];
        
        /// <summary>
        /// Get the length of a <see cref="BezierCurve"/>.
        /// </summary>
        /// <param name="curveIndex">The 0 based index of the curve to find length for.</param>
        /// <returns>The length of the bezier curve at index.</returns>
        [BurstCompile]
        public float GetCurveLength(int curveIndex)
        {
            return SegmentLengthsLookupTable[curveIndex * SegmentResolution + SegmentResolution - 1].Distance;    
        }
        
        /// <summary>
        /// Return the up vector for a t ratio on the curve.
        /// </summary>
        /// <param name="index">The index of the curve for which the length needs to be retrieved.</param>
        /// <param name="t">A value between 0 and 1 representing the ratio along the spline.</param>
        /// <returns>
        /// Returns the up vector at the t ratio of the curve of index 'index'.
        /// </returns>
        public float3 GetCurveUpVector(int index, float t)
        {
            var curveIndex = index * SegmentResolution;
            var offset = 1f / (float)(SegmentResolution - 1);
            var curveT = 0f;
            for (int i = 0; i < SegmentResolution; i++)
            {
                if (t <= curveT + offset)
                {
                    var value = math.lerp(UpVectorsLookupTable[curveIndex + i], 
                        UpVectorsLookupTable[curveIndex + i + 1], 
                        (t - curveT) / offset);
                    
                    return value;
                }
                curveT += offset;
            }

            //Otherwise, no value has been found, return the one at the end of the segment
            return UpVectorsLookupTable[curveIndex + SegmentResolution - 1];
        }
    }
}