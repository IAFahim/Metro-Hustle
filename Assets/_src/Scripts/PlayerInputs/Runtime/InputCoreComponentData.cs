#if !BL_DISABLE_INPUT
using Unity.Entities;
using Unity.Mathematics;

namespace BovineLabs.Core.Input
{
    public partial struct InputCoreComponentData : IComponentData
    {
#if UNITY_EDITOR
        [InputAction]
#endif
        public float2 MoveCurrent;
#if UNITY_EDITOR
        [InputActionDelta]
#endif
        public float2 MoveDelta;
#if UNITY_EDITOR
        [InputActionDown]
#endif
        public bool Sprint;
#if UNITY_EDITOR
        [InputAction]
#endif
        public bool Jump;
    }
}
#endif