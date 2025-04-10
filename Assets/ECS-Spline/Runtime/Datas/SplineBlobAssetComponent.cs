using Unity.Entities;

namespace ECS_Spline.Runtime.Datas
{
    public struct SplineBlobAssetComponent : IComponentData
    {
        public BlobAssetReference<NativeSplineBlob> Reference;
        public float Progress;
    }
}