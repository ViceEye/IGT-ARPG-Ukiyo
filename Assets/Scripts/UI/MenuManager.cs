using System.Collections;
using Ukiyo.Common;
using Ukiyo.UI.Interface;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Ukiyo.UI
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] 
        private GameObject[] targetFadeOutObjects;
        [SerializeField] 
        private GameObject[] targetFadeInObjects;
        [SerializeField] 
        private SliderSync sliderSync;

        private bool allowOpenScene;

        private void OnEnable()
        {
            CircleRefresh.OnCompleted += AllowOpenScene;
        }

        private void AllowOpenScene()
        {
            allowOpenScene = true;
            CircleRefresh.OnCompleted -= AllowOpenScene;
        }

        public void FadeOutObjects()
        {
            foreach (var targetFadeObject in targetFadeOutObjects)
            {
                StartCoroutine(Utils.Zoom(targetFadeObject, new Vector3(0.5f, 0.5f, 0.5f), 1.0f, 0));
                StartCoroutine(Utils.Fade(targetFadeObject, 0.0f, 0.5f, 0));
            }
            Invoke(nameof(FadeInObjects), 1.0f);
        }
        
        private void FadeInObjects()
        {
            foreach (var targetFadeObject in targetFadeInObjects)
            {
                targetFadeObject.SetActive(true);
                StartCoroutine(Utils.Zoom(targetFadeObject, new Vector3(5.0f, 5.0f, 1.0f), 0.5f, 0));
                StartCoroutine(Utils.Fade(targetFadeObject, 1.0f, 1.0f, 0));
            }
            Invoke(nameof(OpenScene), 1.0f);
        }
        
        public void ExitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }

        // If player has hit start button trigger load new scene
        public void OpenScene()
        {
            // Only ran Coroutine when self is active
            if (sliderSync.Slider.IsActive())
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
                // Debug.Log(async.progress);
                float progress = Mathf.Clamp01(async.progress / 0.9f);
                sliderSync.Slider.value = progress;
                
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
