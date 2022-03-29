using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.Common
{
    public static class Utils
    {
        #region UI Animation

        public static IEnumerator Zoom(GameObject transform, Vector3 scale, float duration)
        {
            if (transform != null)
            {
                yield return Zoom(transform.GetComponent<RectTransform>(), scale, duration);
            }
            yield return null;
        }
        
        public static IEnumerator Zoom(RectTransform transform, Vector3 scale, float duration)
        {
            var time = 0.0f;
            var originalScale = transform.localScale;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.localScale = new Vector3(
                    Mathf.Lerp(originalScale.x, scale.x, time / duration),
                    Mathf.Lerp(originalScale.y, scale.y, time / duration),
                    Mathf.Lerp(originalScale.z, scale.z, time / duration));
                yield return new WaitForEndOfFrame();
            }

            transform.localScale = scale;
        }
        
        public static IEnumerator Fade(GameObject group, float alpha, float duration)
        {
            if (group != null)
            {
                yield return Fade(group.GetComponent<CanvasGroup>(), alpha, duration);
            }
            yield return null;
        }

        public static IEnumerator Fade(CanvasGroup group, float alpha, float duration)
        {
            var time = 0.0f;
            var originalAlpha = group.alpha;
            while (time < duration)
            {
                time += Time.deltaTime;
                group.alpha = Mathf.Lerp(originalAlpha, alpha, time / duration);
                yield return new WaitForEndOfFrame();
            }

            group.alpha = alpha;
        }
        
        public static IEnumerator MoveX(GameObject transform, float xValue, float duration)
        {
            if (transform != null)
            {
                yield return MoveX(transform.GetComponent<RectTransform>(), xValue, duration);
            }
            yield return null;
        }
        
        public static IEnumerator MoveX(RectTransform transform, float xValue, float duration)
        {
            var time = 0.0f;
            var originalPosition = transform.localPosition;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.localScale = new Vector3(
                    Mathf.Lerp(originalPosition.x, xValue, time / duration),
                    originalPosition.y,
                    originalPosition.z);
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector3(xValue, originalPosition.y, originalPosition.z);
        }
        
        public static IEnumerator MoveY(GameObject transform, float yValue, float duration)
        {
            if (transform != null)
            {
                yield return MoveY(transform.GetComponent<RectTransform>(), yValue, duration);
            }
            yield return null;
        }
        
        public static IEnumerator MoveY(RectTransform transform, float yValue, float duration)
        {
            var time = 0.0f;
            var originalPosition = transform.localPosition;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.localScale = new Vector3(
                    originalPosition.x,
                    Mathf.Lerp(originalPosition.y, yValue, time / duration),
                    originalPosition.z);
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector3(originalPosition.x, yValue, originalPosition.z);
        }
        
        public static IEnumerator Coloring(GameObject color, Color target, float duration)
        {
            if (color != null)
            {
                yield return Coloring(color.GetComponent<Image>(), target, duration);
            }
            yield return null;
        }

        public static IEnumerator Coloring(Image color, Color target, float duration)
        {
            var time = 0.0f;
            var originalColor = color.color;
            while (time < duration)
            {
                time += Time.deltaTime;
                color.color = Color.Lerp(originalColor, target, time / duration);
                yield return new WaitForEndOfFrame();
            }

            color.color = target;
        }

        #endregion

        #region Tool

        public static void Move<T>(this List<T> list, int oldIndex, int newIndex)
        {
            // exit if positions are equal or outside array
            if ((oldIndex == newIndex) || (0 > oldIndex) || (oldIndex >= list.Count) || (0 > newIndex) ||
                (newIndex >= list.Count)) return;
            // local variables
            var i = 0;
            T tmp = list[oldIndex];
            // move element down and shift other elements up
            if (oldIndex < newIndex)
            {
                for (i = oldIndex; i < newIndex; i++)
                {
                    list[i] = list[i + 1];
                }
            }
            // move element up and shift other elements down
            else
            {
                for (i = oldIndex; i > newIndex; i--)
                {
                    list[i] = list[i - 1];
                }
            }
            // put element from position 1 to destination
            list[newIndex] = tmp;
        }

        #endregion
    }
}
