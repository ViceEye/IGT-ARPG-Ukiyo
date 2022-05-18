using System;
using Ukiyo.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.UI.Interface
{
    public class ExitPanel : MonoBehaviour
    {
        public UIAnimation _inGameAnimation;
        public CanvasGroup _canvasGroup;
        public GraphicRaycaster _graphicRaycaster;
        public UIAnimation _animation;
        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _animation = GetComponent<UIAnimation>();
            _graphicRaycaster = GetComponent<GraphicRaycaster>();
            _graphicRaycaster.enabled = false;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (_canvasGroup.alpha >= 1.0)
                    Close();
                else
                    Open();
            }
        }

        public void Open()
        {
            ThirdPersonController.locking = true;
            _animation.PlayOpenAnimation();
            _inGameAnimation.PlayCloseAnimation();
            _graphicRaycaster.enabled = true;
            Cursor.visible = true;
        }

        public void Close()
        {
            ThirdPersonController.locking = false;
            _animation.PlayCloseAnimation();
            _inGameAnimation.PlayOpenAnimation();
            _graphicRaycaster.enabled = false;
            Cursor.visible = false;
        }
        
        public void ExitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
            Close();
        }
    }
}