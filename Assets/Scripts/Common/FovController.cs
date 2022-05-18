using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Ukiyo.Common.Camera
{
    [ExecuteInEditMode]
    public class FovController : MonoBehaviour
    {
        public CinemachineFreeLook cinemachineFreeLookPar;
        public GameObject player;
        // Cached popup msg
        public InGamePopupMsg.PopupMsg popupMsg = new InGamePopupMsg.PopupMsg("Press G to spawn", -1.0f);
        
        // Settings
        public float duration = 1.0f;
        // Before spawn
        public float defaultFov = 55.0f;
        // After spawn
        public float targetFov = 13.5f;
        
        private void Start()
        {
            // Set default Fov
            cinemachineFreeLookPar = GetComponent<CinemachineFreeLook>();
            cinemachineFreeLookPar.m_Lens.FieldOfView = defaultFov;
        }

        // Check if Fov is updated
        private bool pulled;

        private void Update()
        {
            // Editor mode lock Fov
            if (!Application.isPlaying)
            {
                cinemachineFreeLookPar.m_Lens.FieldOfView = defaultFov;
            }
            else
            {
                // Build mode, notify player and pull the Fov closer when key down
                if (!player.activeSelf)
                {
                    if (InGamePopupMsg.Instance.CheckRemainingTime(popupMsg) > -1.0f)
                    {
                        InGamePopupMsg.Instance.AddText(popupMsg);
                    }
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        player.SetActive(true);
                        Cursor.visible = false;
                        StartCoroutine(PullClose());
                        InGamePopupMsg.Instance.RemoveText(popupMsg);
                    }
                }
                // If editor and player is active
                else if (player.activeSelf && !pulled)
                {
                    pulled = true;
                    StartCoroutine(PullClose());
                    InGamePopupMsg.Instance.RemoveText(popupMsg);
                }
            }
        }

        // Pull camera Fov closer
        IEnumerator PullClose()
        {
            var time = 0.0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                cinemachineFreeLookPar.m_Lens.FieldOfView = Mathf.Lerp(defaultFov, targetFov, time / duration);
                yield return new WaitForEndOfFrame();
            }
            cinemachineFreeLookPar.m_Lens.FieldOfView = targetFov;
        }
    }
}