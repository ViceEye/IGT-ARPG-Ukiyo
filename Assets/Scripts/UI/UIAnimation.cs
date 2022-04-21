﻿using System;
using System.Collections.Generic;
using Ukiyo.Common;
using UnityEngine;

namespace Ukiyo.UI
{
    public class UIAnimation : MonoBehaviour
    {
        public enum AnimationType
        {
            Fade,
            Scale,
            MoveX,
            MoveY,
            Color,
        }

        [Serializable]
        public class UIAnimationSet
        {
            public AnimationType type;
            public float finishValueOnOpen;
            public float finishValueOnClose;
            public bool doReset;
            public float duration;
        }

        public List<UIAnimationSet> ListUIAnimationSet;

        public void PlayOpenAnimation() 
        {
            if (ListUIAnimationSet == null) return;

            foreach (var uiAnimationSet in ListUIAnimationSet)
            {
                switch (uiAnimationSet.type)
                {
                    case AnimationType.Fade:
                        // If do reset, set the value back to original (value on close)
                        if (uiAnimationSet.doReset)
                            GetComponent<CanvasGroup>().alpha = uiAnimationSet.finishValueOnClose;
                        // Run animation
                        StartCoroutine(Utils.Fade(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration));
                        break;
                    case AnimationType.Scale:
                        if (uiAnimationSet.doReset)
                            transform.localScale = Vector3.one * uiAnimationSet.finishValueOnClose;
                        
                        StartCoroutine(Utils.Zoom(gameObject, Vector3.one * uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration));
                        break;
                    case AnimationType.MoveX:
                        if (uiAnimationSet.doReset)
                            transform.localPosition = new Vector3(uiAnimationSet.finishValueOnClose,
                                transform.localPosition.y, transform.localPosition.z);
                        
                        StartCoroutine(Utils.MoveX(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration));
                        break;
                    case AnimationType.MoveY:
                        if (uiAnimationSet.doReset)
                            transform.localPosition = new Vector3(uiAnimationSet.finishValueOnClose,
                                transform.localPosition.y, transform.localPosition.z);
                        
                        StartCoroutine(Utils.MoveY(gameObject, uiAnimationSet.finishValueOnOpen,
                            uiAnimationSet.duration));
                        break;
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
                            uiAnimationSet.duration));
                        break;
                    case AnimationType.Scale:
                        StartCoroutine(Utils.Zoom(gameObject, Vector3.one * uiAnimationSet.finishValueOnClose,
                            uiAnimationSet.duration));
                        break;
                    case AnimationType.MoveX:
                        StartCoroutine(Utils.MoveX(gameObject, uiAnimationSet.finishValueOnClose,
                            uiAnimationSet.duration));
                        break;
                    case AnimationType.MoveY:
                        StartCoroutine(Utils.MoveY(gameObject, uiAnimationSet.finishValueOnClose,
                            uiAnimationSet.duration));
                        break;
                }
            }
        }
    }
}