using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace ECSUnitySplineAddon.Runtime.Datas
{
    /// <summary>
    /// Unmanaged, immutable representation of an entire SplineContainer, baked into an ECS Blob Asset.
    /// Contains all necessary data (knots, curves, LUTs, links) for efficient runtime evaluation of splines.
    /// </summary>
    [BurstCompile]
    public partial struct NativeSplineContainerBlob
    {
        /// <summary>Metadata for each spline stored in the blob.</summary>
        public BlobArray<SplineMetadataInBlob> SplineMetadatas;

        /// <summary>Flattened array of all knots from all valid splines.</summary>
        public BlobArray<BezierKnot> AllKnots;

        /// <summary>Flattened array of all curves (segments between knots) from all valid splines.</summary>
        public BlobArray<BezierCurve> AllCurves;

        /// <summary>Lookup table mapping curve distance to curve interpolation factor (T).</summary>
        public BlobArray<DistanceToInterpolation> DistanceLUT;

        /// <summary>Optional lookup table for pre-calculated up-vectors along curves (using RMF).</summary>
        public BlobArray<float3> UpVectorLUT;

        /// <summary>Metadata describing groups of linked knots.</summary>
        public BlobArray<LinkGroupMetadataInBlob> LinkGroupMetadatas;

        /// <summary>Flattened array of all knot links (BlobSplineKnotIndex).</summary>
        public BlobArray<BlobSplineKnotIndex> AllLinks;

        /// <summary>The resolution (number of samples per curve) used for the DistanceLUT and UpVectorLUT.</summary>
        public int DistanceLutResolution;

        /// <summary>Gets the number of splines stored in this blob.</summary>
        public int SplineCount => SplineMetadatas.Length;

        /// <summary>Gets the read-only metadata for a specific spline within the blob.</summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <returns>A reference to the spline's metadata.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if splineIndex is invalid.</exception>
        public ref readonly SplineMetadataInBlob GetSplineMetadata(int splineIndex)
        {
            if (splineIndex < 0 || splineIndex >= SplineMetadatas.Length)
                throw new System.IndexOutOfRangeException(
                    $"Invalid splineIndex: {splineIndex}. Must be between 0 and {SplineMetadatas.Length - 1}.");
            return ref SplineMetadatas[splineIndex];
        }

        /// <summary>Gets a specific knot belonging to a specific spline.</summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="knotIndexInSpline">The index of the knot within that specific spline.</param>
        /// <returns>The BezierKnot data. Returns default if indices are invalid.</returns>
        public BezierKnot GetKnot(int splineIndex, int knotIndexInSpline)
        {
            ref readonly var meta = ref GetSplineMetadata(splineIndex);
            int globalKnotIndex = meta.KnotStartIndex + knotIndexInSpline;
            if (knotIndexInSpline < 0 || knotIndexInSpline >= meta.KnotCount || globalKnotIndex >= AllKnots.Length)
            {
                return default;
            }

            return AllKnots[globalKnotIndex];
        }

        /// <summary>Gets a specific curve belonging to a specific spline.</summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="curveIndexInSpline">The index of the curve within that specific spline (0 to CurveCount-1).</param>
        /// <returns>The BezierCurve data. Returns default if indices are invalid.</returns>
        public BezierCurve GetCurve(int splineIndex, int curveIndexInSpline)
        {
            ref readonly var meta = ref GetSplineMetadata(splineIndex);
            int globalCurveIndex = meta.CurveStartIndex + curveIndexInSpline;
            if (curveIndexInSpline < 0 || curveIndexInSpline >= meta.CurveCount || globalCurveIndex >= AllCurves.Length)
            {
                return default;
            }

            return AllCurves[globalCurveIndex];
        }

        /// <summary>Gets the approximate length of a specific curve within a specific spline, using the Distance LUT.</summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="curveIndexInSpline">The index of the curve within that specific spline.</param>
        /// <returns>The approximate length of the curve. Returns 0 if indices are invalid or LUT is unusable.</returns>
        public float GetCurveLength(int splineIndex, int curveIndexInSpline)
        {
            ref readonly var meta = ref GetSplineMetadata(splineIndex);
            if (curveIndexInSpline < 0 || curveIndexInSpline >= meta.CurveCount || DistanceLutResolution <= 0 ||
                meta.DistLutStartIndex < 0)
                return 0f;

            int lutLastEntryIndex = meta.DistLutStartIndex + curveIndexInSpline * DistanceLutResolution +
                                    (DistanceLutResolution - 1);

            if (lutLastEntryIndex < 0 || lutLastEntryIndex >= DistanceLUT.Length) return 0f;

            return DistanceLUT[lutLastEntryIndex].Distance;
        }

        /// <summary>
        /// Converts a normalized interpolation value (0-1) for a *specific spline* into the curve index
        /// within that spline and a normalized interpolation value (0-1) along that curve.
        /// This uses the pre-calculated curve lengths derived from the Distance LUT.
        /// </summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="splineT">The normalized time (0-1) along the entire spline.</param>
        /// <param name="curveT">Outputs the normalized time (0-1) along the identified curve.</param>
        /// <returns>The index of the curve within the spline that corresponds to splineT.</returns>
        public int SplineToCurveT(int splineIndex, float splineT, out float curveT)
        {
            ref readonly var meta = ref GetSplineMetadata(splineIndex);
            if (meta.KnotCount <= 1 || meta.CurveCount == 0)
            {
                curveT = 0f;
                return 0;
            }

            splineT = math.clamp(splineT, 0f, 1f);
            float totalSplineLength = meta.Length;
            if (totalSplineLength <= 0.0001f)
            {
                curveT = splineT;
                return 0;
            }

            float targetDistance = splineT * totalSplineLength;
            float accumulatedDistance = 0f;

            for (int curveIdxInSpline = 0; curveIdxInSpline < meta.CurveCount; ++curveIdxInSpline)
            {
                float currentCurveLength = GetCurveLength(splineIndex, curveIdxInSpline);
                float nextAccumulatedDistance = accumulatedDistance + currentCurveLength;

                if (targetDistance <= nextAccumulatedDistance + 0.0001f)
                {
                    float distanceIntoCurve = targetDistance - accumulatedDistance;
                    GetCurveInterpolationFromDistance(
                        splineIndex, curveIdxInSpline, distanceIntoCurve, currentCurveLength,
                        out curveT
                    );
                    return curveIdxInSpline;
                }

                accumulatedDistance = nextAccumulatedDistance;
            }

            curveT = 1f;
            return meta.CurveCount - 1;
        }

        /// <summary>
        /// Gets the curve-local interpolation factor (T) from a distance along that specific curve, using the Distance LUT.
        /// </summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="curveIndexInSpline">The index of the curve within that specific spline.</param>
        /// <param name="distanceInCurve">The distance along the curve.</param>
        /// <param name="curveLength">The total length of the curve (passed in for potential optimization).</param>
        /// <param name="interpolationNormal">The normalized interpolation factor (0-1) along the curve.</param>
        [BurstCompile]
        private void GetCurveInterpolationFromDistance(int splineIndex, int curveIndexInSpline, float distanceInCurve,
            float curveLength, out float interpolationNormal)
        {
            if (distanceInCurve <= 0.0001f) interpolationNormal = 0f;
            if (distanceInCurve >= curveLength - 0.0001f) interpolationNormal = 1f;

            ref readonly var meta = ref GetSplineMetadata(splineIndex);
            if (meta.DistLutStartIndex < 0 || DistanceLutResolution <= 1) interpolationNormal = 0f;

            int lutStartIndex = meta.DistLutStartIndex + curveIndexInSpline * DistanceLutResolution;
            int lutEndIndex = lutStartIndex + DistanceLutResolution - 1;

            if (lutStartIndex < 0 || lutEndIndex >= DistanceLUT.Length) interpolationNormal = 0f;

            for (int i = 0; i < DistanceLutResolution - 1; ++i)
            {
                int currentLutIndex = lutStartIndex + i;
                ref readonly var prev = ref DistanceLUT[currentLutIndex];
                ref readonly var next = ref DistanceLUT[currentLutIndex + 1];

                if (distanceInCurve >= prev.Distance && distanceInCurve <= next.Distance)
                {
                    float segmentLength = next.Distance - prev.Distance;
                    if (segmentLength <= 0.00001f) interpolationNormal = prev.T;

                    float lerpFactor = (distanceInCurve - prev.Distance) / segmentLength;
                    interpolationNormal = math.lerp(prev.T, next.T, lerpFactor);
                }
            }

            interpolationNormal = DistanceLUT[lutEndIndex].T;
        }


        /// <summary>Evaluates the position on a specific spline at a normalized spline time t (0-1).</summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="splineT">The normalized time (0-1) along the entire spline.</param>
        /// <param name="position">The calculated position in the space the spline was baked in (local or world).</param>
        [BurstCompile]
        public void EvaluatePosition(int splineIndex, float splineT, out float3 position)
        {
            ref readonly var meta = ref GetSplineMetadata(splineIndex);
            if (meta.KnotCount == 0) position = float3.zero;
            if (meta.KnotCount == 1) position = GetKnot(splineIndex, 0).Position;

            int curveIdx = SplineToCurveT(splineIndex, splineT, out float curveT);
            BezierCurve curve = GetCurve(splineIndex, curveIdx);
            EvaluatePositionInternal(curve, curveT, out position);
        }

        /// <summary>Evaluates the tangent (first derivative) on a specific spline at a normalized spline time t (0-1).</summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="splineT">The normalized time (0-1) along the entire spline.</param>
        /// <param name="tangentNoNormal">The calculated tangent vector (not normalized). Represents direction and speed.</param>
        [BurstCompile]
        public void EvaluateTangent(int splineIndex, float splineT, out float3 tangentNoNormal)
        {
            ref readonly var meta = ref GetSplineMetadata(splineIndex);
            if (meta.KnotCount < 2) tangentNoNormal = new float3(0, 0, 1);

            int curveIdx = SplineToCurveT(splineIndex, splineT, out float curveT);
            BezierCurve curve = GetCurve(splineIndex, curveIdx);
            EvaluateTangentInternal(curve, curveT, out tangentNoNormal);
        }

        /// <summary>Evaluates the up-vector on a specific spline at a normalized spline time t (0-1).</summary>
        /// <remarks>
        /// Uses the pre-calculated UpVectorLUT if available and valid. Otherwise, falls back to
        /// calculating it dynamically using the adapted internal RMF logic, which depends on internal APIs.
        /// </remarks>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="splineT">The normalized time (0-1) along the entire spline.</param>
        /// <param name="upVector">The calculated up-vector (normalized).</param>
        [BurstCompile]
        public void EvaluateUpVector(int splineIndex, float splineT, out float3 upVector)
        {
            ref readonly var meta = ref GetSplineMetadata(splineIndex);
            if (meta.KnotCount == 0) upVector = math.up();
            if (meta.KnotCount == 1) upVector = math.rotate(GetKnot(splineIndex, 0).Rotation, math.up());
            if (meta.CurveCount == 0) upVector = math.up();

            int curveIdx = SplineToCurveT(splineIndex, splineT, out float curveT);

            bool canUseLut = meta.UpVectorLutStartIndex >= 0 &&
                             UpVectorLUT.Length > 0 &&
                             DistanceLutResolution > 1 &&
                             meta.UpVectorLutStartIndex + (curveIdx + 1) * DistanceLutResolution <= UpVectorLUT.Length;

            if (canUseLut)
            {
                int lutStartIndex = meta.UpVectorLutStartIndex + curveIdx * DistanceLutResolution;
                int lutResolution = DistanceLutResolution;

                float segmentT = curveT * (lutResolution - 1);
                int index0 = (int)math.floor(segmentT);
                index0 = math.clamp(index0, 0, lutResolution - 2);
                int index1 = index0 + 1;

                float lerpFactor = segmentT - index0;

                int globalIndex0 = lutStartIndex + index0;
                int globalIndex1 = lutStartIndex + index1;

                if (globalIndex0 >= 0 && globalIndex1 < UpVectorLUT.Length)
                {
                    upVector = math.normalizesafe(
                        Vector3.Slerp(UpVectorLUT[globalIndex0], UpVectorLUT[globalIndex1], lerpFactor), math.up());
                }
            }

            BezierCurve curve = GetCurve(splineIndex, curveIdx);

            BezierKnot knotStart = GetKnot(splineIndex, curveIdx);
            int endKnotIndexInSpline = meta.Closed
                ? (curveIdx + 1) % meta.KnotCount
                : math.min(curveIdx + 1, meta.KnotCount - 1);
            BezierKnot knotEnd = GetKnot(splineIndex, endKnotIndexInSpline);

            float3 startUp = math.rotate(knotStart.Rotation, math.up());
            float3 endUp = math.rotate(knotEnd.Rotation, math.up());

            upVector = CurveUtilityInternal.EvaluateUpVector(curve, curveT, startUp, endUp);
        }

        /// <summary>
        /// Evaluates the position, tangent (unnormalized), and up-vector (normalized)
        /// on a specific spline at a normalized spline time t (0-1). This is often more
        /// efficient than calling EvaluatePosition, EvaluateTangent, and EvaluateUpVector separately.
        /// </summary>
        /// <param name="splineIndex">The index of the spline (relative to this blob).</param>
        /// <param name="splineT">The normalized time (0-1) along the entire spline.</param>
        /// <param name="position">Outputs the calculated position.</param>
        /// <param name="tangent">Outputs the calculated tangent (unnormalized).</param>
        /// <param name="upVector">Outputs the calculated up-vector (normalized).</param>
        [BurstCompile]
        public void Evaluate(int splineIndex, float splineT, out float3 position, out float3 tangent,
            out float3 upVector)
        {
            ref readonly var meta = ref GetSplineMetadata(splineIndex);

            if (meta.KnotCount == 0)
            {
                position = float3.zero;
                tangent = new float3(0, 0, 1);
                upVector = new float3(0, 1, 0);
                return;
            }

            if (meta.KnotCount == 1)
            {
                BezierKnot knot = GetKnot(splineIndex, 0);
                position = knot.Position;
                tangent = math.mul(knot.Rotation, new float3(0, 0, 1));
                upVector = math.mul(knot.Rotation, new float3(0, 1, 0));
                return;
            }

            if (meta.CurveCount == 0)
            {
                BezierKnot knot = GetKnot(splineIndex, 0);
                position = knot.Position;
                tangent = math.mul(knot.Rotation, new float3(0, 0, 1));
                upVector = math.mul(knot.Rotation, new float3(0, 1, 0));
                return;
            }

            int curveIdx = SplineToCurveT(splineIndex, splineT, out float curveT);
            BezierCurve curve = GetCurve(splineIndex, curveIdx);

            EvaluatePositionInternal(curve, curveT, out position);
            EvaluateTangentInternal(curve, curveT, out tangent);

            bool canUseLut = meta.UpVectorLutStartIndex >= 0 &&
                             UpVectorLUT.Length > 0 &&
                             DistanceLutResolution > 1 &&
                             meta.UpVectorLutStartIndex + (curveIdx + 1) * DistanceLutResolution <= UpVectorLUT.Length;

            if (canUseLut)
            {
                int lutStartIndex = meta.UpVectorLutStartIndex + curveIdx * DistanceLutResolution;
                int lutResolution = DistanceLutResolution;
                float segmentT = curveT * (lutResolution - 1);
                int index0 = math.clamp((int)math.floor(segmentT), 0, lutResolution - 2);
                int index1 = index0 + 1;
                float lerpFactor = segmentT - index0;
                int globalIndex0 = lutStartIndex + index0;
                int globalIndex1 = lutStartIndex + index1;

                if (globalIndex0 >= 0 && globalIndex1 < UpVectorLUT.Length)
                {
                    upVector = math.normalizesafe(
                        Vector3.Slerp(UpVectorLUT[globalIndex0], UpVectorLUT[globalIndex1], lerpFactor), math.up());
                    return;
                }
            }

            BezierKnot knotStart = GetKnot(splineIndex, curveIdx);
            int endKnotIndexInSpline = meta.Closed
                ? (curveIdx + 1) % meta.KnotCount
                : math.min(curveIdx + 1, meta.KnotCount - 1);
            BezierKnot knotEnd = GetKnot(splineIndex, endKnotIndexInSpline);
            float3 startUp = math.rotate(knotStart.Rotation, math.up());
            float3 endUp = math.rotate(knotEnd.Rotation, math.up());
            upVector = CurveUtilityInternal.EvaluateUpVector(curve, curveT, startUp, endUp);
        }


        /// <summary>Internal helper to evaluate position on a single Bezier curve.</summary>
        /// <param name="curve">The Bezier curve data.</param>
        /// <param name="t">Normalized time (0-1) along this curve.</param>
        /// <param name="positionInCurve">Position on the curve.</param>
        [BurstCompile]
        private static void EvaluatePositionInternal(in BezierCurve curve, float t, out float3 positionInCurve)
        {
            t = math.clamp(t, 0f, 1f);
            float mt = 1f - t;
            float mt2 = mt * mt;
            float t2 = t * t;
            positionInCurve = (mt2 * mt * curve.P0) +
                              (3f * mt2 * t * curve.P1) +
                              (3f * mt * t2 * curve.P2) +
                              (t2 * t * curve.P3);
        }

        /// <summary>Internal helper to evaluate tangent on a single Bezier curve.</summary>
        /// <param name="curve">The Bezier curve data.</param>
        /// <param name="t">Normalized time (0-1) along this curve.</param>
        /// <param name="tangent">Tangent vector (unnormalized).</param>
        [BurstCompile]
        private static void EvaluateTangentInternal(in BezierCurve curve, float t, out float3 tangent)
        {
            t = math.clamp(t, 0f, 1f);
            float mt = 1.0f - t;
            tangent = (3f * mt * mt * (curve.P1 - curve.P0)) +
                      (6f * mt * t * (curve.P2 - curve.P1)) +
                      (3f * t * t * (curve.P3 - curve.P2));
        }

        #region Knot Link Accessors

        /// <summary>Gets the number of distinct knot link groups stored in the blob.</summary>
        public int LinkGroupCount => LinkGroupMetadatas.Length;

        /// <summary>Gets the metadata for a specific link group.</summary>
        /// <param name="groupIndex">The index of the link group (0 to LinkGroupCount - 1).</param>
        /// <returns>Read-only reference to the link group metadata.</returns>
        /// <exception cref="System.IndexOutOfRangeException">Thrown if groupIndex is invalid.</exception>
        public ref readonly LinkGroupMetadataInBlob GetLinkGroupMetadata(int groupIndex)
        {
            if (groupIndex < 0 || groupIndex >= LinkGroupMetadatas.Length)
                throw new System.IndexOutOfRangeException(
                    $"Invalid link group index: {groupIndex}. Must be between 0 and {LinkGroupMetadatas.Length - 1}.");
            return ref LinkGroupMetadatas[groupIndex];
        }

        /// <summary>
        /// Gets a specific knot link (BlobSplineKnotIndex) from the flattened link array using group metadata.
        /// </summary>
        /// <param name="groupIndex">The index of the link group.</param>
        /// <param name="linkIndexInGroup">The index of the link within that specific group (0 to LinkCount - 1).</param>
        /// <returns>The BlobSplineKnotIndex representing the linked knot. Returns default if indices are invalid.</returns>
        public BlobSplineKnotIndex GetLink(int groupIndex, int linkIndexInGroup)
        {
            ref readonly var groupMeta = ref GetLinkGroupMetadata(groupIndex);
            int globalLinkIndex = groupMeta.LinkStartIndex + linkIndexInGroup;
            if (linkIndexInGroup < 0 || linkIndexInGroup >= groupMeta.LinkCount || globalLinkIndex >= AllLinks.Length)
            {
                return default;
            }

            return AllLinks[globalLinkIndex];
        }

        #endregion
    }
}