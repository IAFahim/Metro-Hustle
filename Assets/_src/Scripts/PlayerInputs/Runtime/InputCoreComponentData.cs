#if !BL_DISABLE_INPUT
using Unity.Entities;
using Unity.Mathematics;

namespace BovineLabs.Core.Input
{
    public partial struct InputCoreComponentData : IComponentData
    {
        [InputAction] public float2 MoveCurrent;
        [InputActionDelta] public float2 MoveDelta;
        [InputActionDown] public bool Sprint;
        [InputAction] public bool Jump;
    }
}
#endif