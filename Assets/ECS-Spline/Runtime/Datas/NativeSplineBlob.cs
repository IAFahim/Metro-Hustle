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
        public BlobArray<BezierKnot> knots;
        public bool closed;
        public float4x4 transformMatrix;

        public static BlobAssetReference<NativeSplineBlob> CreateNativeSplineBlobAssetRef
        (
            NativeSpline nativeSpline,
            bool isClosed,
            float4x4 transformMatrix
        )
        {
            // Riping values
            var knots = nativeSpline.Knots;

            // Constructing blob
            using var nativeSplineBuilder = new BlobBuilder(Allocator.Temp);
            ref var nativeSplineRoot = ref nativeSplineBuilder.ConstructRoot<NativeSplineBlob>();

            var knotsBuilder = nativeSplineBuilder.Allocate(ref nativeSplineRoot.knots, knots.Length);
            for (int i = 0; i < knots.Length; i++)
            {
                knotsBuilder[i] = knots[i];
            }

            nativeSplineRoot.closed = isClosed;
            nativeSplineRoot.transformMatrix = transformMatrix;

            return nativeSplineBuilder
                .CreateBlobAssetReference<NativeSplineBlob>(Allocator.Persistent);
        }
    }

    public readonly struct KnotsReadonlyCollection : IReadOnlyList<BezierKnot>
    {
        private readonly NativeList<BezierKnot> _knots;

        public KnotsReadonlyCollection(NativeList<BezierKnot> knots)
        {
            _knots = knots;
        }

        public IEnumerator<BezierKnot> GetEnumerator()
        {
            for (var i = 0; i < _knots.Length; i++)
            {
                yield return _knots[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public BezierKnot this[int index] => _knots[index];
        public int Count => _knots.Length;
    }
}