// In your UI assembly (e.g., YourProject.UI)

using BovineLabs.Anchor;
using BovineLabs.Anchor.Contracts;
using Unity.Properties;

namespace _src.Scripts.Healths.Healths.Data
{
    public partial class HealthViewModel : SystemObservableObject<HealthViewModel.Data>
    {
        [CreateProperty(ReadOnly = true)]
        public int CurrentHealth => this.Value.CurrentHealth;

        [CreateProperty(ReadOnly = true)]
        public int MaxHealth => this.Value.MaxHealth;

        [CreateProperty(ReadOnly = true)]
        public string HealthText => $"{this.CurrentHealth} / {this.MaxHealth}";

        [CreateProperty(ReadOnly = true)]
        public float HealthNormalized => this.MaxHealth > 0 ? (float)this.CurrentHealth / this.MaxHealth : 0f;

        public partial struct Data
        {
            [SystemProperty] private int currentHealth;
            [SystemProperty] private int maxHealth;
        }
    }
}