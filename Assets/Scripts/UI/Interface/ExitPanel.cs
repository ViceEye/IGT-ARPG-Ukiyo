using System;
using UnityEngine;

namespace Ukiyo.UI.Interface
{
    public class ExitPanel : MonoBehaviour
    {
        public UIAnimation _inGameAnimation;
        public CanvasGroup _canvasGroup;
        public UIAnimation _animation;
        
        private void Start()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            _animation = GetComponent<UIAnimation>();
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
            _animation.PlayOpenAnimation();
            _inGameAnimation.PlayCloseAnimation();
        }

        public void Close()
        {
            _animation.PlayCloseAnimation();
            _inGameAnimation.PlayOpenAnimation();
        }
        
        public void ExitGame()
        {
            Debug.Log("Quit");
            Application.Quit();
            Close();
        }
    }
}