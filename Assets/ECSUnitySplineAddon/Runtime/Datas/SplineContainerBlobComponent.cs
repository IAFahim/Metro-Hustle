using Unity.Entities;

namespace ECSUnitySplineAddon.Runtime.Datas
{
    /// <summary>
    /// ECS Component holding a reference to the baked NativeSplineContainerBlob asset.
    /// Represents an entire SplineContainer. Added to an entity during baking.
    /// </summary>
    public partial struct SplineContainerBlobComponent : IComponentData
    {
        public BlobAssetReference<NativeSplineContainerBlob> Value;
    }
}