using BovineLabs.Anchor;
using Unity.AppUI.MVVM;

namespace _src.Scripts.UiTookKit_ECS.Runtime.Classes
{
    public class GameAppBuilder : GameAppBuilder<AnchorApp>
    {
        
    }
    public class GameAppBuilder<T> : UIToolkitAppBuilder<T> where T : App
    {
        
    }
}
