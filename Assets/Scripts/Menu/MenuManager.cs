using UnityEngine;

namespace Ukiyo.Menu
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] 
        private GameObject[] targetFadeOutObjects;
        [SerializeField] 
        private GameObject[] targetFadeInObjects;
        [SerializeField] 
        private SliderSync slider;

        public void FadeOutObjects()
        {
            foreach (var targetFadeObject in targetFadeOutObjects)
            {
                StartCoroutine(Utils.Zoom(targetFadeObject, new Vector3(0.5f, 0.5f, 0.5f), 1.0f));
                StartCoroutine(Utils.Fade(targetFadeObject, 0.0f, 0.5f));
            }
            Invoke(nameof(FadeInObjects), 1.0f);
        }
        
        private void FadeInObjects()
        {
            foreach (var targetFadeObject in targetFadeInObjects)
            {
                targetFadeObject.SetActive(true);
                StartCoroutine(Utils.Zoom(targetFadeObject, new Vector3(5.0f, 5.0f, 1.0f), 0.5f));
                StartCoroutine(Utils.Fade(targetFadeObject, 1.0f, 1.0f));
            }
            Invoke(nameof(OpenScene), 1.0f);
        }

        private void OpenScene()
        {
            slider.OpenScene();
        }
        
        public void ExitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
        }
    }
}
