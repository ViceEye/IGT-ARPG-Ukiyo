using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.UI.Interface
{
    // Status Bar
    public class PlayerHealthMana : MonoBehaviour
    {
        public static PlayerHealthMana Instance;

        private void Start()
        {
            if (Instance == null)
            {
                Instance = this;
            }
        }

        public List<Slider> _healthMana = new List<Slider>();
        public List<Text> _healthManaText = new List<Text>();

        public void SetValue(EnumHealthMana type, double value, double maxValue)
        {
            int key = (int) type;
            if (value <= maxValue)
            {
                double percentage = value / maxValue;
                _healthMana[key].value = (float) percentage;
                _healthManaText[key].text = $"{(int) value}/{(int) maxValue}";
            }
        }
    }

    public enum EnumHealthMana
    {
        Health,
        Mana
    }
}