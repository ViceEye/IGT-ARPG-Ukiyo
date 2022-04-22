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
        [SerializeField]
        private Text percentage;
        [SerializeField]
        private Text loading;
        [SerializeField] 
        private Image refresh;

        private void OnEnable()
        {
            CircleRefresh.OnCompleted += AllowOpenScene;
        }

        private int cycleCount;
        private bool allowOpenScene;

        private void AllowOpenScene()
        {
            allowOpenScene = true;
            CircleRefresh.OnCompleted -= AllowOpenScene;
        }

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

        // If player has hit start button trigger load new scene
        public void OpenScene()
        {
            // Only ran Coroutine when self is active
            if (slider.IsActive())
                StartCoroutine(AysncLoadNewScene("DebugScene"));
        }

        private IEnumerator AysncLoadNewScene(string sceneName)
        {
            yield return new WaitForSeconds(0.5f);
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

            if (async == null) yield break;
            
            // Do not active scene as soon as it loaded
            async.allowSceneActivation = false;
            // While the operation to load the new scene is not complete, continue waiting and update progress percentage
            while (!async.isDone)
            {
                float progress = Mathf.Clamp01(async.progress / 0.9f);
                slider.value = progress;
                    
                // When the scene is loaded, wait 3s and load scene
                if (progress >= 1.0f)
                {
                    if (allowOpenScene)
                    {
                        // Load next scene
                        async.allowSceneActivation = true;
                    }
                }
                    
                yield return null;
            }
        }
    }
}
