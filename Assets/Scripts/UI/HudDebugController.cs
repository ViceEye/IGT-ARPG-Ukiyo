using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class HudDebugController : MonoBehaviour
{
    [Range(0.0f, 1.0f)] 
    public float percentage;

    private void OnDrawGizmos()
    {
        
        // Ensure continuous Update calls.
        if (Application.isPlaying) return;
        
        // Call frequently
        // UnityEditor.EditorApplication.QueuePlayerLoopUpdate();
            
        // Call when changes
        UnityEditor.EditorApplication.delayCall += UnityEditor.EditorApplication.QueuePlayerLoopUpdate;
        UnityEditor.SceneView.RepaintAll();
        
    }

    private void Update()
    {
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
