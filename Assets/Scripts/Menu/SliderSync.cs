using System;
using System.Collections;
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

        private void Update()
        {
            int value = (int) (slider.value * 100);
            percentage.text = value.ToString() + "%";
        }

        public void OpenScene()
        {
            StartCoroutine(AysncLoadNewScene("Demo"));
        }

        IEnumerator AysncLoadNewScene(string sceneName)
        {
            yield return new WaitForSeconds(1);

            AsyncOperation async = SceneManager.LoadSceneAsync(sceneName);

            if (async != null)
            {
                while (!async.isDone)
                {
                    float progress = Mathf.Clamp01(async.progress / 0.9f);
                    slider.value = progress;
                    yield return null;
                }
            }
        }
    }
}
