using System.Collections.Generic;
using LitJson;
using Ukiyo.Common;
using UnityEditor;
using UnityEngine;

namespace Ukiyo.Serializable
{
#if UNITY_EDITOR
    public class DataEditor : MonoBehaviour
    {
        
        private string savaDataFilePath = "/Resources/SaveData/";
        private string itemsFileName = "EquipmentItemsData.json";
        
        [SerializeField]
        private List<ObjectData> listItemDataSettings;
        [SerializeField]
        private List<ObjectData> previewListItemDataSettings;

        [ContextMenu("Test")]
        public void Test()
        {
            Utils.GetResourcePath(this);
        }
        
        [ContextMenu("GenerateItemJsonFile")]
        private void GenerateItemJsonFile()
        {
            List<ObjectDataJson> listItemJsons = new List<ObjectDataJson>();
            foreach (var listItemDataSetting in listItemDataSettings)
            {
                listItemJsons.Add(new ObjectDataJson(listItemDataSetting));
            }
            Utils.WriteIntoFile(listItemJsons, savaDataFilePath, itemsFileName);
            Debug.Log("Generated Item Json");
        }

        [ContextMenu("LoadItemsFromJsonFile")]
        private void LoadItemsFromJsonFile()
        {
            List<ObjectData> listItems = new List<ObjectData>();

            string allItemsJson = Utils.GetJsonStr(savaDataFilePath, itemsFileName);
            
            if (allItemsJson != "" && allItemsJson != "[{}]")
            {
                JsonData allItemsJsonData = JsonMapper.ToObject(allItemsJson);

                foreach (JsonData jsonData in allItemsJsonData)
                {
                    ObjectData objectData = new ObjectData
                    {
                        ID = int.Parse(jsonData["ID"].ToString()),
                        DisplayName = jsonData["DisplayName"].ToString(),
                        Icon = Utils.LoadResource<Sprite>(jsonData["Icon"].ToString()),
                        Capacity = int.Parse(jsonData["Capacity"].ToString()),
                        Description = jsonData["Description"].ToString(),
                        Type = (EnumInventoryItemType) int.Parse(jsonData["Type"].ToString())
                    };
                    listItems.Add(objectData);
                }

                previewListItemDataSettings = listItems;
            }
        }

    }

    class ObjectDataJson
    {
        public int ID { get; set; }

        public string DisplayName { get; set; }

        public string Icon { get; set; }
        
        public int Capacity { get; set; }

        public string Description { get; set; }

        public EnumInventoryItemType Type { get; set; }
        
        public ObjectDataJson(ObjectData objectData)
        {
            ID = objectData.ID;
            DisplayName = objectData.DisplayName;
            Icon = Utils.GetResourcePath(objectData.Icon);
            Capacity = objectData.Capacity;
            Description = objectData.Description;
            Type = objectData.Type;
        }
    }
#endif
}