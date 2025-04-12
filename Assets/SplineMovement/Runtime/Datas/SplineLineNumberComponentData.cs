using Unity.Entities;

namespace SplineMovement.Runtime.Datas
{
    public struct SplineLineNumberComponentData : IComponentData, IEnableableComponent
    {
        public sbyte Value;
    }
}