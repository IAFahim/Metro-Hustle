// using _src.Scripts.Speeds.Runtime.Datas;
// using ECS_Spline.Runtime.Datas;
// using SplineMovement.Runtime.Datas;
// using Unity.Collections;
// using Unity.Entities;
// using Unity.Transforms;
// using UnityEngine.Splines;
//
// namespace SplineMovement.Runtime.Systems
// {
//     public partial struct SplineForwardMovementJobEntity : IJobEntity
//     {
//         [ReadOnly] public LocalTransform SplineLocalTransform;
//         [ReadOnly] public BlobAssetReference<NativeSplineBlob> NativeSplineBlob;
//
//         private void Execute(ref LocalTransform localTransform, ref SplineLinkComponentData splineLinkComponentData,
//             in SpeedComponentData speedComponentData
//         )
//         {
//             if (splineLinkComponentData.Progress >= 1)
//             {
//                 splineLinkComponentData.Progress = 0;
//                 splineLinkComponentData.KnotIndex++;
//             }
//
//             splineLinkComponentData.Progress += speedComponentData.GetCurrentSpeed();
//             var progress = splineLinkComponentData.Progress;
//             var valueROKnotIndex = splineLinkComponentData.KnotIndex;
//
//             var curveOffset = CurveUtility.EvaluatePosition(NativeSplineBlob.Value.Curves[valueROKnotIndex], progress);
//             var positionOnCurve = curveOffset + SplineLocalTransform.Position;
//             localTransform.Position = positionOnCurve;
//         }
//     }
// }