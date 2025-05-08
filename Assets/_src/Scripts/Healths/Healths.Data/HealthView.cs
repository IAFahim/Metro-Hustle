// In your UI assembly (e.g., YourProject.UI)

using _src.Scripts.Healths.Healths.Data;
using BovineLabs.Anchor;
using Unity.AppUI.UI;
using UnityEngine.UIElements;

[IsService]
public class HealthView : View<HealthViewModel>
{
    public const string UssClassName = "health-view";

    public HealthView(HealthViewModel viewModel) : base(viewModel)
    {
        this.AddToClassList(UssClassName);
        this.style.flexDirection = FlexDirection.Row;

        var healthLabel = new Text { name = "health-label", text = "Health: " };
        this.Add(healthLabel);

        var healthTextValue = new Text { name = "health-text-value" };
        healthTextValue.dataSource = this.ViewModel;
        healthTextValue.SetBindingToUI(nameof(healthTextValue.text), nameof(HealthViewModel.CurrentHealth));
        this.Add(healthTextValue);

        var healthBar = new ProgressBar { name = "health-bar", lowValue = 0, highValue = 1 };
        healthBar.style.minWidth = 100; healthBar.style.height = 15;
        healthBar.dataSource = this.ViewModel;
        healthBar.SetBindingToUI(nameof(ProgressBar.value), nameof(HealthViewModel.HealthNormalized));
        this.Add(healthBar);
    }
}