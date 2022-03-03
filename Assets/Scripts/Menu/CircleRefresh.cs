using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.Menu
{
    [ExecuteInEditMode]
    public class CircleRefresh : MonoBehaviour
    {
        public delegate void RefreshCircleCompleted();
        public static event RefreshCircleCompleted OnCompleted;
        
        [SerializeField]
        private Image image;
        
        // Start is called before the first frame update
        void Start()
        {
            image = GetComponent<Image>();
            image.fillAmount = 0;
            image.fillClockwise = true;
        }

        private bool inverse;
        // Update is called once per frame
        void Update()
        {
            if (inverse)
                image.fillAmount -= Time.deltaTime;
            else
                image.fillAmount += Time.deltaTime;

            if (image.fillAmount >= 1.0 && !inverse)
            {
                inverse = true;
                image.fillClockwise = false;
            }
            else if (image.fillAmount <= 0 && inverse)
            {
                inverse = false;
                image.fillClockwise = true;

                OnCompleted?.Invoke();
            }
        }
    }
}
