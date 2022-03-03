using System;
using System.Collections;
using System.Timers;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ukiyo.Menu
{
    [ExecuteInEditMode]
    public class SliderSync : MonoBehaviour
    {
        public Slider slider;
        public Text percentage;
        public Text loading;
        private bool switcher;

        private void Start()
        {
            InvokeRepeating(nameof(Loading), 0, 0.5f);
        }

        private void Update()
        {
            var value = (int) (slider.value * 100);
            percentage.text = value + "%";
        }

        private void Loading()
        {
            var value = (int) (slider.value * 100);
            if (value != 100)
            {
                loading.text = switcher ? loading.text.Substring(0, loading.text.Length -1) : loading.text += ".";
                switcher = !switcher;
            }
            else
            {
                loading.text = "Completed";
                CancelInvoke(nameof(Loading));
            }
        }

        // If player has hit start button trigger load new scene
        public void OpenScene()
        {
            if (slider.IsActive())
                StartCoroutine(AysncLoadNewScene("Demo"));
        }

        private IEnumerator AysncLoadNewScene(string sceneName)
        {
            yield return new WaitForSeconds(1);
            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

            if (async != null)
            {
                // Do not active scene as soon as it loaded
                async.allowSceneActivation = false;
                // While the operation to load the new scene is not complete, continue waiting and update progress percentage
                while (!async.isDone)
                {
                    float progress = Mathf.Clamp01(async.progress / 0.9f);
                    slider.value = progress;
                    
                    // When the scene is loaded, wait 3s and load scene
                    // todo: Add transition animation/images
                    if (progress >= 1.0f)
                    {
                        yield return new WaitForSeconds(3);
                        // Load next scene
                        async.allowSceneActivation = true;
                    }
                    
                    yield return null;
                }

            }
        }
    }
}
