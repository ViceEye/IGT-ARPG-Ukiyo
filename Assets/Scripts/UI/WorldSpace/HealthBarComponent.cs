using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.UI.WorldSpace
{
    // In world enemy health bar
    public class HealthBarComponent : MonoBehaviour
    {
        public Slider _healthSlider;

        private void Start()
        {
            _healthSlider = GetComponentInChildren<Slider>();
        }

        public void SetHealth(double health, double maxHealth)
        {
            if (health <= maxHealth)
            {
                double percentage = health / maxHealth;
                _healthSlider.value = (float) percentage;
            }
        }
    }
}