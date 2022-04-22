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
        // Aysnc user interface animation
        #region UI Animations

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
                transform.localPosition = new Vector3(
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
                transform.localPosition = new Vector3(
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
                yield return Coloring(color.GetComponent<Graphic>(), target, duration);
            }
            yield return null;
        }

        public static IEnumerator Coloring(Graphic color, Color target, float duration)
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
            string filePath = Application.dataPath + savaDataFilePath;

            Debug.Log(str);
            File.WriteAllText(filePath + fileName, str);
        }

        public static string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.None).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }

        public static T LoadResource<T>(string path) where T : UnityEngine.Object
        {
            Debug.Log(path);
            T obj = null;
            try
            {
                obj = Resources.Load<T>(path);
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            Debug.Log(obj);
            return obj;
        }

#if UNITY_EDITOR
        public static string GetResourcePath(UnityEngine.Object obj)
        {
            string fullPath = AssetDatabase.GetAssetPath(obj);
            Debug.Log(fullPath);
            string resourcePath = fullPath.Replace("Assets/Resources/", "");
            return resourcePath.Substring(0, resourcePath.LastIndexOf("."));
        }
#endif

        #endregion
    }
}
