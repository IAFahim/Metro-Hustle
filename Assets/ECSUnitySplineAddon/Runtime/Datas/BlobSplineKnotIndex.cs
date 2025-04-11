namespace ECSUnitySplineAddon.Runtime.Datas
{
    /// <summary>
    /// Unmanaged representation of SplineKnotIndex (Spline + Knot index pair) for use in Blobs.
    /// Indices refer to the spline's position within the *blob* and the knot's position within *that spline's knots*.
    /// </summary>
    public struct BlobSplineKnotIndex
    {
        public int SplineIndex;
        public int KnotIndex;
    }
}