using Unity.Entities;

namespace ECS_Spline.Runtime.Datas
{
    public struct NativeSplineBlobComponentData : IComponentData
    {
        public BlobAssetReference<NativeSplineBlob> Value;
    }
}