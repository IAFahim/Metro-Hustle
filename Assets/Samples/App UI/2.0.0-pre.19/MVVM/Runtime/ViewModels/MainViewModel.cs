using System;
using Unity.AppUI.MVVM;
#if UNITY_2023_2_OR_NEWER
using Unity.Properties;
#endif

namespace Unity.AppUI.Samples.MVVM
{
    [ObservableObject]
    public partial class MainViewModel
    {
        [ObservableProperty]
        [AlsoNotifyChangeFor(nameof(ClickCountMessage))]
        int m_Counter;

        [ObservableProperty] [AlsoNotifyChangeFor(nameof(ClickCountMessage))]
        private int m_other_counter;

        public MainViewModel()
        {
            Counter = 0;
        }

        [ICommand]
        void IncrementCounter()
        {
            Counter++;
        }
        
        [ICommand]
        void DecrementCounter()
        {
            Counter--;
        }

        // When creating properties yourself, you must
        // use CreateProperty attribute for UITK data binding to work.
#if UNITY_2023_2_OR_NEWER
        [CreateProperty(ReadOnly = true)]
#endif
        public string ClickCountMessage => $"Click count: {Counter}";
    }
}
