#if !BL_DISABLE_INPUT
using Unity.Entities;
using Unity.Mathematics;

namespace BovineLabs.Core.Input
{
    public partial struct InputDirectionComponentData : IComponentData
    {
        [InputActionDown] public float3 Value;
    }
}
#endif