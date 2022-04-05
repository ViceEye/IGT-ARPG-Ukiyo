using System.Collections.Generic;
using LitJson;
using Ukiyo.Common;
using UnityEditor;
using UnityEngine;

namespace Ukiyo.Serializable
{
    public class DataEditor : MonoBehaviour
    {
        
        private string savaDataFilePath = "/Resources/SaveData/";
        private string itemsFileName = "EquipmentItemsData.json";
        
        [SerializeField]
        private List<ObjectData> listItemDataSettings;
        [SerializeField]
        private List<ObjectData> previewListItemDataSettings;

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
                        __id = int.Parse(jsonData["ID"].ToString()),
                        __displayName = jsonData["DisplayName"].ToString(),
                        __icon = Utils.LoadResource<Sprite>(jsonData["Icon"].ToString()),
                        __description = jsonData["Description"].ToString(),
                        __type = (EnumInventoryItemType) int.Parse(jsonData["Type"].ToString())
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

        public string Description { get; set; }

        public EnumInventoryItemType Type { get; set; }
        
        public ObjectDataJson(ObjectData objectData)
        {
            ID = objectData.ID;
            DisplayName = objectData.DisplayName;
            Icon = AssetDatabase.GetAssetPath(objectData.Icon);
            Description = objectData.Description;
            Type = objectData.Type;
        }
    }
}