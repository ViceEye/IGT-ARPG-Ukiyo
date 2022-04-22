using System;
using System.Collections.Generic;
using LitJson;
using Ukiyo.Common.Singleton;
using Ukiyo.Serializable;
using UnityEngine;

namespace Ukiyo.Common.Object
{
    public class ObjectPool : MonoSingleton<ObjectPool>, IObject
    {
        private string savaDataFilePath = "/Resources/SaveData/";
        private string itemsFileName = "EquipmentItemsData.json";
        
        private readonly Dictionary<int, ObjectData> itemPool = new Dictionary<int, ObjectData>();

        public void Init()
        {
            LoadItemsFromJsonFile();
        }

        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }

        // Load Al Items From Json File and cache to Item Pool
        private void LoadItemsFromJsonFile()
        {
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
                    // Cache item to item pool
                    if (!itemPool.ContainsKey(objectData.ID))
                        itemPool.Add(objectData.ID, objectData);
                }
            }
        }

        public ObjectData GetItemById(int id)
        {
            if (itemPool.ContainsKey(id))
            {
                return itemPool[id];
            }
            return null;
        }
        
    }
}