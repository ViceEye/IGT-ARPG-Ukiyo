using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using LitJson;
using UnityEngine;

namespace Ukiyo.Serializable
{
    public class DataEditor : MonoBehaviour
    {
        
        private string savaDataFilePath = "/Resources/SaveData/";
        private string itemsFileName = "EquipmentItemsData.json";
        
        [SerializeField]
        private List<ObjectData> listItemDataSettings;

        [ContextMenu("GenerateItemJsonFile")]
        private void GenerateItemJsonFile()
        {
            WriteIntoFile(listItemDataSettings, itemsFileName);
            Debug.Log("Generated");
        }

        private void WriteIntoFile(object obj, string fileName)
        {
            string str = JsonMapper.ToJson(obj);
            str = Unicode2String(str);
            string filePath = Application.dataPath + savaDataFilePath;

            File.WriteAllText(filePath + fileName, str);
        }

        private string Unicode2String(string source)
        {
            return new Regex(@"\\u([0-9A-F]{4})", RegexOptions.IgnoreCase | RegexOptions.None).Replace(
                source, x => string.Empty + Convert.ToChar(Convert.ToUInt16(x.Result("$1"), 16)));
        }
        
    }
}