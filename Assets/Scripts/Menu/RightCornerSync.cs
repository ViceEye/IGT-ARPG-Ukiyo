using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

namespace Ukiyo.Menu
{
    [ExecuteInEditMode]
    public class RightCornerSync : MonoBehaviour
    {

        public int offset = 20;
        public Slider slider;
        public RectTransform sliderRect;
        public RectTransform selfRect;
        
        void Update()
        {
            var position = selfRect.position;
            //                       Adjust the right corner as slider value changes
            //                                                            * 0.975f for adjust the gap problem
            position = new Vector3(sliderRect.rect.width * slider.value * 0.975f + offset, position.y, position.z);
            selfRect.position = position;
        }
    }
}
