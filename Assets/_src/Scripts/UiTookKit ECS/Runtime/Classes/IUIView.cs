namespace _src.Scripts.UiTookKit_ECS.Runtime.Classes
{
    public interface IUIView
    {
    }

    public interface IUIView<out T> : IUIView
    {
        T ViewModel { get; }
    }
}