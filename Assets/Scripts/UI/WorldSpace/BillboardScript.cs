using UnityEngine;

namespace Ukiyo.UI.WorldSpace
{
    public class BillboardScript : MonoBehaviour
    {
        public float visibleRadius = 7.0f;
        public Camera _camara;
        public CanvasGroup _canvasGroup;

        private void Start()
        {
            _camara = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            _canvasGroup = GetComponent<CanvasGroup>();
            _canvasGroup.alpha = 0.0f;
        }

        private void Update()
        {
            GameObject player = GameObject.FindWithTag("Player");
            if (player != null && player.activeSelf)
            {
                if (Vector3.Distance(player.transform.position, transform.position) <= visibleRadius)
                {
                    _canvasGroup.alpha = 1.0f;
                    var rotation = _camara.transform.rotation;
                    // Always facing player camera
                    transform.LookAt(transform.position + rotation * Vector3.back, rotation * Vector3.up);
                }
                else
                {
                    _canvasGroup.alpha = 0.0f;
                }
            }
        }
    }
}