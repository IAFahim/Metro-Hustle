namespace ECSUnitySplineAddon.Runtime.Datas
{
    /// <summary>
    /// Metadata for a single spline stored within the NativeSplineContainerBlob.
    /// </summary>
    public struct SplineMetadataInBlob
    {
        public int KnotStartIndex;
        public int KnotCount;
        public int CurveStartIndex;
        public int CurveCount;
        public int DistLutStartIndex;
        public int UpVectorLutStartIndex;
        public float Length;
        public bool Closed;
    }
}