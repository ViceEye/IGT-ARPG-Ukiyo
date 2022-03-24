using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ukiyo.Utils
{
    public static class Utils
    {
        #region UI

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
