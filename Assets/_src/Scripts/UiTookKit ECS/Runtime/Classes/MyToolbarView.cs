using BovineLabs.Anchor;
using BovineLabs.Anchor.Binding;
using BovineLabs.Anchor.Contracts;
using BovineLabs.Anchor.Toolbar;
using Unity.AppUI.UI;
using Unity.Properties;
using UnityEngine.UIElements;

namespace _src.Scripts.UiTookKit_ECS.Runtime.Classes
{
    [AutoToolbar("Test")]
    public class MyToolbarView : VisualElement, IView<MyToolbarViewModel>
    {
        public MyToolbarView()
        {
            // Create UI elements and bind to ViewModel
            var counter = new Text();
            counter.dataSource = this.ViewModel;
            counter.SetBindingToUI(nameof(Text.text), nameof(MyToolbarViewModel.Counter));
            this.Add(counter);
        }

        // ViewModel can be injected if uses services but generally you just create it directly
        public MyToolbarViewModel ViewModel { get; } = new();
    }

    public partial class MyToolbarViewModel : SystemObservableObject<MyToolbarViewModel.Data>
    {
        [CreateProperty(ReadOnly = true)] public int Counter => this.Value.Counter;

        public partial struct Data
        {
            [SystemProperty] public int counter;
        }
    }
}



