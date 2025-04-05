#if !BL_DISABLE_INPUT
using BovineLabs.Core.Input;
using Unity.Entities;
using Unity.Mathematics;

namespace BovineLabs.Core.Input
{
    public partial struct InputMapComponentData : IComponentData
    {
        [InputAction] public float2 MoveCurrent;
        [InputActionDelta] public float2 LookDelta;
        [InputActionDown] public bool Sprint;
        [InputAction] public bool Jump;
    }
}
#endif