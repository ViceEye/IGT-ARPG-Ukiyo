using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ukiyo.UI
{
    [ExecuteInEditMode]
    public class SliderSync : MonoBehaviour
    {
        [SerializeField]
        private CanvasGroup canvasGroup;
        [SerializeField]
        private Slider slider;
        public Slider Slider => slider;
        [SerializeField]
        private Text percentage;
        [SerializeField]
        private Text loading;
        [SerializeField] 
        private Image refresh;

        private int cycleCount;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            slider = GetComponent<Slider>();
            percentage = slider.GetComponentsInChildren<Text>()[0];
            loading = slider.GetComponentsInChildren<Text>()[1];
            
            // Update loading text every 0.5s
            loading.text = "Loading...";
            InvokeRepeating(nameof(Loading), 0, 0.5f);
        }

        private void Update()
        {
            var value = (int) (slider.value * 100);
            percentage.text = value + "%";
        }

        private bool switcher;
        private void Loading()
        {
            var value = (int) (slider.value * 100);
            if (value != 100)
            {
                // Loading text dot change effect
                loading.text = switcher ? loading.text.Substring(0, loading.text.Length -1) : loading.text += ".";
                switcher = !switcher;
            }
            else
            {
                loading.text = "Completed";
                CancelInvoke(nameof(Loading));
                StartCoroutine(FadeInRefreshHideSelf());
            }
        }

        private IEnumerator FadeInRefreshHideSelf()
        {
            yield return new WaitForSeconds(0.2f);
            canvasGroup.alpha = 0;
            if (refresh != null)
            {
                refresh.gameObject.SetActive(true);
            }
        }
    }
}
