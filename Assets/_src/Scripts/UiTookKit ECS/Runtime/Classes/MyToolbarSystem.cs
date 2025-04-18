using BovineLabs.Anchor.Toolbar;
using Unity.Burst;
using Unity.Entities;

namespace _src.Scripts.UiTookKit_ECS.Runtime.Classes
{
    [UpdateInGroup(typeof(ToolbarSystemGroup))]
    public partial struct MyToolbarSystem : ISystem, ISystemStartStop
    {
        private ToolbarHelper<MyToolbarView, MyToolbarViewModel, MyToolbarViewModel.Data> toolbar;
    
        public void OnCreate(ref SystemState state)
        {
            this.toolbar = new ToolbarHelper<MyToolbarView, MyToolbarViewModel, MyToolbarViewModel.Data>(ref state, "MyTab");
        }
    
        public void OnStartRunning(ref SystemState state)
        {
            this.toolbar.Load();
        }
    
        public void OnStopRunning(ref SystemState state)
        {
            this.toolbar.Unload();
        }
    
        [BurstCompile]
        public void OnUpdate(ref SystemState state)
        {
            if (!this.toolbar.IsVisible())
            {
                return;
            }
        
            ref var data = ref this.toolbar.Binding;
            data.counter++; // Access ViewModel data directly
        }
    }
}