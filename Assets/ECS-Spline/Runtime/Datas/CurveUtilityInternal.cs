using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

namespace ECS_Spline.Runtime.Datas
{
    /// <summary>
    /// Exposing CurveUtility internal Method
    /// </summary>
    internal static class CurveUtilityInternal
    {
        struct FrenetFrame
        {
            public float3 origin;
            public float3 tangent;
            public float3 normal;
            public float3 binormal;
        }

        const int k_NormalsPerCurve = 16;
        
        const float k_Epsilon = 0.0001f;
        
        static bool Approximately(float a, float b)
        {
            return math.abs(b - a) < math.max(0.000001f * math.max(math.abs(a), math.abs(b)), k_Epsilon * 8);
        }

        internal static void EvaluateUpVectors(BezierCurve curve, float3 startUp, float3 endUp, NativeArray<float3> upVectors)
        {
            upVectors[0] = startUp;
            upVectors[upVectors.Length - 1] = endUp;

            for(int i = 1; i < upVectors.Length - 1; i++)
            {
                var curveT = i / (float)(upVectors.Length - 1);
                upVectors[i] = EvaluateUpVector(curve, curveT, upVectors[0], endUp);
            }
        }
        internal static float3 GetExplicitLinearTangent(float3 point, float3 to)
        {
            return (to - point) / 3.0f;
        }
        
        internal static float3 EvaluateUpVector(BezierCurve curve, float t, float3 startUp, float3 endUp,
            bool fixEndUpMismatch = true)
        {
            var linearTangentLen = math.length(GetExplicitLinearTangent(curve.P0, curve.P3));
            var linearTangentOut = math.normalize(curve.P3 - curve.P0) * linearTangentLen;
            if (Approximately(math.length(curve.P1 - curve.P0), 0f))
                curve.P1 = curve.P0 + linearTangentOut;
            if (Approximately(math.length(curve.P2 - curve.P3), 0f))
                curve.P2 = curve.P3 - linearTangentOut;

            var normalBuffer = new NativeArray<float3>(k_NormalsPerCurve, Allocator.Temp);
            
            FrenetFrame frame;
            frame.origin = curve.P0;
            frame.tangent = curve.P1 - curve.P0;
            frame.normal = startUp;
            frame.binormal = math.normalize(math.cross(frame.tangent, frame.normal));
            if(float.IsNaN(frame.binormal.x))
                return float3.zero;
            
            normalBuffer[0] = frame.normal;
            
            var stepSize = 1f / (k_NormalsPerCurve - 1);
            var currentT = stepSize;
            var prevT = 0f;
            var upVector = float3.zero;
            FrenetFrame prevFrame;
            for (int i = 1; i < k_NormalsPerCurve; ++i)
            {
                prevFrame = frame;
                frame = GetNextRotationMinimizingFrame(curve, prevFrame, currentT);                
                
                normalBuffer[i] = frame.normal;

                if (prevT <= t && currentT >= t)
                {
                    var lerpT = (t - prevT) / stepSize;
                    upVector = Vector3.Slerp(prevFrame.normal, frame.normal, lerpT);
                }

                prevT = currentT;
                currentT += stepSize;
            }

            if (!fixEndUpMismatch)
                return upVector;

            if (prevT <= t && currentT >= t)
                upVector = endUp;

            var lastFrameNormal = normalBuffer[k_NormalsPerCurve - 1];

            var angleBetweenNormals = math.acos(math.clamp(math.dot(lastFrameNormal, endUp), -1f, 1f));
            if (angleBetweenNormals == 0f)
                return upVector;

            var lastNormalTangent = math.normalize(frame.tangent);
            var positiveRotation = quaternion.AxisAngle(lastNormalTangent, angleBetweenNormals);
            var negativeRotation = quaternion.AxisAngle(lastNormalTangent, -angleBetweenNormals);
            var positiveRotationResult = math.acos(math.clamp(math.dot(math.rotate(positiveRotation, endUp), lastFrameNormal), -1f, 1f));
            var negativeRotationResult = math.acos(math.clamp(math.dot(math.rotate(negativeRotation, endUp), lastFrameNormal), -1f, 1f));

            if (positiveRotationResult > negativeRotationResult)
                angleBetweenNormals *= -1f;

            currentT = stepSize;
            prevT = 0f;
            
            for (int i = 1; i < normalBuffer.Length; i++)
            {
                var normal = normalBuffer[i];
                var adjustmentAngle = math.lerp(0f, angleBetweenNormals, currentT);
                var tangent = math.normalize(CurveUtility.EvaluateTangent(curve, currentT));
                var adjustedNormal = math.rotate(quaternion.AxisAngle(tangent, -adjustmentAngle), normal);

                normalBuffer[i] = adjustedNormal;

                if (prevT <= t && currentT >= t)
                {
                    var lerpT = (t - prevT) / stepSize;
                    upVector = Vector3.Slerp(normalBuffer[i - 1], normalBuffer[i], lerpT);

                    return upVector;
                }

                prevT = currentT;
                currentT += stepSize;
            }

            return endUp;
        }

        static FrenetFrame GetNextRotationMinimizingFrame(BezierCurve curve, FrenetFrame previousRMFrame, float nextRMFrameT)
        {
            FrenetFrame nextRMFrame;
            nextRMFrame.origin = CurveUtility.EvaluatePosition(curve, nextRMFrameT);
            nextRMFrame.tangent = CurveUtility.EvaluateTangent(curve, nextRMFrameT);

            float3 toCurrentFrame = nextRMFrame.origin - previousRMFrame.origin;
            float c1 = math.dot(toCurrentFrame, toCurrentFrame);
            float3 riL = previousRMFrame.binormal - toCurrentFrame * 2f / c1 * math.dot(toCurrentFrame, previousRMFrame.binormal);
            float3 tiL = previousRMFrame.tangent - toCurrentFrame * 2f / c1 * math.dot(toCurrentFrame, previousRMFrame.tangent);

            float3 v2 = nextRMFrame.tangent - tiL;
            float c2 = math.dot(v2, v2);

            nextRMFrame.binormal = math.normalize(riL - v2 * 2f / c2 * math.dot(v2, riL));
            nextRMFrame.normal = math.normalize(math.cross(nextRMFrame.binormal, nextRMFrame.tangent));

            return nextRMFrame;
        }

    }
}
