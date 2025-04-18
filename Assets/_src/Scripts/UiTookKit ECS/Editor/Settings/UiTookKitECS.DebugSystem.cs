#if Aline
using Drawing;
#endif


using Unity.Burst;
using Unity.Entities;

namespace Scripts.UiTookKitECS.Editor
{
    [BurstCompile]
    public partial struct UiTookKitECSDebugSystem : ISystem
    {
        [BurstCompile]
        public void OnCreate(ref SystemState state)
        {
        }

        public void OnUpdate(ref SystemState state)
        {
#if Aline
        
#endif
        }

        [BurstCompile]
        public void OnDestroy(ref SystemState state)
        {
        }
    }
}