using System;
using System.Collections.Generic;
using Ukiyo.Common;
using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.UI
{
    public class UIAnimation : MonoBehaviour
    {
        public enum AnimationType
        {
            Fade,
            Scale,
            ScaleX,
            ScaleY,
            MoveX,
            MoveY,
            Fill,
            //Color,
        }

        [Serializable]
        public class UIAnimationSet
        {
            public AnimationType type;
            public float finishValueOnOpen;
            public float finishValueOnClose;
            public bool doReset;
            public float openDelay;
            public float closeDelay;
            public float duration;
        }

        public bool autoPlay;
        public List<UIAnimationSet> ListUIAnimationSet;

        private void Start()
        {
            if (autoPlay)
                PlayOpenAnimation();
        }

        public void PlayOpenAnimation() 
        {
            if (ListUIAnimationSet == null) return;

            foreach (var uiAnimationSet in ListUIAnimationSet)
            {
                switch (uiAnimationSet.type)
                {
                    case AnimationType.Fade:
                    {
                        // If do reset, set the value back to original (value on close)
                        if (uiAnimationSet.doReset)
                            GetComponent<CanvasGroup>().alpha = uiAnimationSet.finishValueOnClose;
                        // Run animation
                        StartCoroutine(Utils.Fade(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration, uiAnimationSet.openDelay));
                        break;
                    }
                    
                    case AnimationType.Scale:
                    {
                        if (uiAnimationSet.doReset)
                            transform.localScale = Vector3.one * uiAnimationSet.finishValueOnClose;

                        StartCoroutine(Utils.Zoom(gameObject, Vector3.one * uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration, uiAnimationSet.openDelay));
                        break;
                    }
                    
                    case AnimationType.ScaleX:
                    {
                        if (uiAnimationSet.doReset)
                            transform.localScale = new Vector3(uiAnimationSet.finishValueOnClose,
                                transform.localScale.y, transform.localScale.z);

                        StartCoroutine(Utils.ScaleX(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration, uiAnimationSet.openDelay));
                        break;
                    }
                    
                    case AnimationType.ScaleY:
                    {
                        if (uiAnimationSet.doReset)
                            transform.localScale = new Vector3(transform.localScale.x,
                                uiAnimationSet.finishValueOnClose, transform.localScale.z);

                        StartCoroutine(Utils.ScaleY(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration, uiAnimationSet.openDelay));
                        break;
                    }
                    
                    case AnimationType.MoveX:
                    {
                        if (uiAnimationSet.doReset)
                            transform.localPosition = new Vector3(uiAnimationSet.finishValueOnClose,
                                transform.localPosition.y, transform.localPosition.z);

                        StartCoroutine(Utils.MoveX(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration, uiAnimationSet.openDelay));
                        break;
                    }
                    
                    case AnimationType.MoveY:
                    {
                        if (uiAnimationSet.doReset)
                            transform.localPosition = new Vector3(transform.localPosition.x,
                                uiAnimationSet.finishValueOnClose, transform.localPosition.z);

                        StartCoroutine(Utils.MoveY(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration, uiAnimationSet.openDelay));
                        break;
                    }

                    case AnimationType.Fill:
                    {
                        if (uiAnimationSet.doReset)
                            GetComponent<Image>().fillAmount = uiAnimationSet.finishValueOnClose;

                        StartCoroutine(Utils.Filling(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration, uiAnimationSet.openDelay));
                        break;
                    }
                }
            }
        }

        public void PlayCloseAnimation()
        {
            if (ListUIAnimationSet == null) return;

            foreach (var uiAnimationSet in ListUIAnimationSet)
            {
                switch (uiAnimationSet.type)
                {
                    case AnimationType.Fade:
                        StartCoroutine(Utils.Fade(gameObject, uiAnimationSet.finishValueOnClose,
                            uiAnimationSet.duration, uiAnimationSet.closeDelay));
                        break;
                    case AnimationType.Scale:
                        StartCoroutine(Utils.Zoom(gameObject, Vector3.one * uiAnimationSet.finishValueOnClose,
                            uiAnimationSet.duration, uiAnimationSet.closeDelay));
                        break;
                    case AnimationType.MoveX:
                        StartCoroutine(Utils.MoveX(gameObject, uiAnimationSet.finishValueOnClose,
                            uiAnimationSet.duration, uiAnimationSet.closeDelay));
                        break;
                    case AnimationType.MoveY:
                        StartCoroutine(Utils.MoveY(gameObject, uiAnimationSet.finishValueOnClose,
                            uiAnimationSet.duration, uiAnimationSet.closeDelay));
                        break;
                    case AnimationType.Fill:
                        StartCoroutine(Utils.MoveY(gameObject, uiAnimationSet.finishValueOnClose,
                            uiAnimationSet.duration, uiAnimationSet.closeDelay));
                        break;
                }
            }
        }
    }
}