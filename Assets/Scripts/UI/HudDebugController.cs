using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.UI
{
    [ExecuteAlways]
    public class HudDebugController : MonoBehaviour
    {
        [Range(0.0f, 1.0f)] 
        public float percentage;
        public bool enable;

        private void OnDrawGizmos()
        {
        
            // Ensure continuous Update calls.
            if (Application.isPlaying) return;
        
            // Call frequently
            // UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            
            // Call when changes
            # if (UNITY_EDITOR)
            {  
                UnityEditor.EditorApplication.delayCall += UnityEditor.EditorApplication.QueuePlayerLoopUpdate;
                UnityEditor.SceneView.RepaintAll();
            }
            #endif
        }

        private void Update()
        {
            if (!enable) return;

            if (Application.isPlaying) return;
        
            var canvasGroups = GetComponentsInChildren<CanvasGroup>();
            foreach (var canvasGroup in canvasGroups)
            {
                canvasGroup.alpha = percentage;
            }

            var images = GetComponentsInChildren<Image>();
            foreach (var image in images)
            {
                image.fillAmount = percentage;
            }
        }
    }

}