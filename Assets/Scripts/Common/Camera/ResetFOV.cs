using System;
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
        public float duration = 1.0f;
        
        private void Start()
        {
            cinemachineFreeLookPar = GetComponent<CinemachineFreeLook>();
            cinemachineFreeLookPar.m_Lens.FieldOfView = 55;
        }

        private bool pulled = false;

        private void Update()
        {
            if (!Application.isPlaying)
            {
                cinemachineFreeLookPar.m_Lens.FieldOfView = 55;
            }
            else
            {
                if (!player.activeSelf)
                {
                    if (Input.GetKeyDown(KeyCode.G))
                    {
                        player.SetActive(true);
                        StartCoroutine(PullClose());
                    }
                }
                else if (player.activeSelf && !pulled)
                {
                    pulled = true;
                    StartCoroutine(PullClose());
                }
            }
        }

        IEnumerator PullClose()
        {
            var time = 0.0f;
            while (time < duration)
            {
                time += Time.deltaTime;
                cinemachineFreeLookPar.m_Lens.FieldOfView = Mathf.Lerp(55, 18, time / duration);
                yield return new WaitForEndOfFrame();
            }
            cinemachineFreeLookPar.m_Lens.FieldOfView = 18;
        }
    }
}