using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
using LitJson;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Ukiyo.Common
{
    public static class Utils
    {
        // Aysnc user interface animation, similar to DoTween
        #region UI Animations

        public static IEnumerator Zoom(GameObject transform, Vector3 scale, float duration, float delay)
        {
            if (transform != null)
            {
                yield return Zoom(transform.GetComponent<RectTransform>(), scale, duration, delay);
            }
            yield return null;
        }
        
        public static IEnumerator Zoom(RectTransform transform, Vector3 scale, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
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
        
        public static IEnumerator ScaleX(GameObject transform, float xValue, float duration, float delay)
        {
            if (transform != null)
            {
                yield return ScaleX(transform.GetComponent<RectTransform>(), xValue, duration, delay);
            }
            yield return null;
        }
        
        public static IEnumerator ScaleX(RectTransform transform, float xValue, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            var time = 0.0f;
            var originalPosition = transform.localScale;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.localScale = new Vector3(
                    Mathf.Lerp(originalPosition.x, xValue, time / duration),
                    originalPosition.y,
                    originalPosition.z);
                yield return new WaitForEndOfFrame();
            }

            transform.localScale = new Vector3(xValue, originalPosition.y, originalPosition.z);
        }
        
        public static IEnumerator ScaleY(GameObject transform, float yValue, float duration, float delay)
        {
            if (transform != null)
            {
                yield return ScaleY(transform.GetComponent<RectTransform>(), yValue, duration, delay);
            }
            yield return null;
        }
        
        public static IEnumerator ScaleY(RectTransform transform, float yValue, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            var time = 0.0f;
            var originalPosition = transform.localScale;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.localScale = new Vector3(
                    originalPosition.x,
                    Mathf.Lerp(originalPosition.y, yValue, time / duration),
                    originalPosition.z);
                yield return new WaitForEndOfFrame();
            }

            transform.localScale = new Vector3(originalPosition.x, yValue, originalPosition.z);
        }
        
        public static IEnumerator Fade(GameObject group, float alpha, float duration, float delay)
        {
            if (group != null)
            {
                yield return Fade(group.GetComponent<CanvasGroup>(), alpha, duration, delay);
            }
            yield return null;
        }

        public static IEnumerator Fade(CanvasGroup group, float alpha, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
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
        
        public static IEnumerator MoveX(GameObject transform, float xValue, float duration, float delay)
        {
            if (transform != null)
            {
                yield return MoveX(transform.GetComponent<RectTransform>(), xValue, duration, delay);
            }
            yield return null;
        }
        
        public static IEnumerator MoveX(RectTransform transform, float xValue, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            var time = 0.0f;
            var originalPosition = transform.localPosition;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.localPosition = new Vector3(
                    Mathf.Lerp(originalPosition.x, xValue, time / duration),
                    originalPosition.y,
                    originalPosition.z);
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector3(xValue, originalPosition.y, originalPosition.z);
        }
        
        public static IEnumerator MoveY(GameObject transform, float yValue, float duration, float delay)
        {
            if (transform != null)
            {
                yield return MoveY(transform.GetComponent<RectTransform>(), yValue, duration, delay);
            }
            yield return null;
        }
        
        public static IEnumerator MoveY(RectTransform transform, float yValue, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            var time = 0.0f;
            var originalPosition = transform.localPosition;
            while (time < duration)
            {
                time += Time.deltaTime;
                transform.localPosition = new Vector3(
                    originalPosition.x,
                    Mathf.Lerp(originalPosition.y, yValue, time / duration),
                    originalPosition.z);
                yield return new WaitForEndOfFrame();
            }

            transform.localPosition = new Vector3(originalPosition.x, yValue, originalPosition.z);
        }
        
        public static IEnumerator Coloring(GameObject color, Color target, float duration, float delay)
        {
            if (color != null)
            {
                yield return Coloring(color.GetComponent<Graphic>(), target, duration, delay);
            }
            yield return null;
        }

        public static IEnumerator Coloring(Graphic color, Color target, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
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
        
        public static IEnumerator Filling(GameObject image, float target, float duration, float delay)
        {
            if (image != null)
            {
                yield return Filling(image.GetComponent<Image>(), target, duration, delay);
            }
            yield return null;
        }

        public static IEnumerator Filling(Image image, float target, float duration, float delay)
        {
            yield return new WaitForSeconds(delay);
            var time = 0.0f;
            var originalFillAmount = image.fillAmount;
            while (time < duration)
            {
                time += Time.deltaTime;
                image.fillAmount = Mathf.Lerp(originalFillAmount, target, time / duration);
                yield return new WaitForEndOfFrame();
            }

            image.fillAmount = target;
        }

        #endregion

        #region Tools
        
        public static string GetJsonStr(string savaDataFilePath, string fileName)
        {
            string filePath = Application.dataPath + savaDataFilePath + fileName;
            
            string jsonStr = string.Empty;

            if (File.Exists(filePath))
            {
                jsonStr = File.ReadAllText(filePath);
            }
            
            return jsonStr;
        }
        
        public static void WriteIntoFile(object obj, string savaDataFilePath, string fileName)
        {
            string str = JsonMapper.ToJson(obj);
            str = Unicode2String(str);
            string path = Application.dataPath + savaDataFilePath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                Debug.LogError("Create Path");
            }
            
            string filePath = Application.dataPath + savaDataFilePath + fileName;
            if (!File.Exists(filePath))
            {
                File.Create(filePath).Close();
                Debug.LogError("Create File");
            }

            File.WriteAllText(filePath, str);
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.None).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        public static T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            T obj = null;
            try
            {
                obj = Resources.Load<T>(path);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return obj;
        }

#if UNITY_EDITOR
        public static string GetResourcePath(UnityEngine.Object obj)
        {
            string fullPath = AssetDatabase.GetAssetPath(obj);
            string resourcePath = fullPath.Replace("Assets/Resources/", "");
            return resourcePath.Substring(0, resourcePath.LastIndexOf("."));
        }
#endif

        #endregion
    }
}
