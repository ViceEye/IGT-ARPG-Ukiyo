using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Ukiyo.Common.Camera
{
    [ExecuteInEditMode]
    public class ResetFOV : MonoBehaviour
    {
        public CinemachineFreeLook cinemachineFreeLookPar;
        public GameObject player;

        public InGamePopupMsg inGamePopupMsg;
        public InGamePopupMsg.PopupMsg popupMsg = new InGamePopupMsg.PopupMsg("Press G to spawn", -1.0f);
        
        public float duration = 1.0f;
        public float defaultFov = 55.0f;
        
        private void Start()
        {
            cinemachineFreeLookPar = GetComponent<CinemachineFreeLook>();
            cinemachineFreeLookPar.m_Lens.FieldOfView = defaultFov;
        }

        private bool pulled = false;

        private void Update()
        {
            if (!Application.isPlaying)
            {
                cinemachineFreeLookPar.m_Lens.FieldOfView = defaultFov;
            }
            else
            {
                if (!player.activeSelf)
                {
                    if (inGamePopupMsg.CheckRemainingTime(popupMsg) > -1.0f)
                    {
                        inGamePopupMsg.AddText(popupMsg);
                    }
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        player.SetActive(true);
                        Cursor.visible = false;
                        StartCoroutine(PullClose());
                        inGamePopupMsg.RemoveText(popupMsg);
                    }
                }
                else if (player.activeSelf && !pulled)
                {
                    pulled = true;
                    StartCoroutine(PullClose());
                    inGamePopupMsg.RemoveText(popupMsg);
                }
            }
        }

        IEnumerator PullClose()
        {
            var time = 0.0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                cinemachineFreeLookPar.m_Lens.FieldOfView = Mathf.Lerp(defaultFov, 18, time / duration);
                yield return new WaitForEndOfFrame();
            }
            cinemachineFreeLookPar.m_Lens.FieldOfView = 18;
        }
    }
}