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
                        __id = int.Parse(jsonData["ID"].ToString()),
                        __displayName = jsonData["DisplayName"].ToString(),
                        __icon = Utils.LoadResource<Sprite>(jsonData["Icon"].ToString()),
                        __description = jsonData["Description"].ToString(),
                        __type = (EnumInventoryItemType) int.Parse(jsonData["Type"].ToString())
                    };
                    // Deserialize object or Capital access will be none.
                    objectData.OnAfterDeserialize();
                    if (!itemPool.ContainsKey(objectData.ID))
                    {
                        itemPool.Add(objectData.ID, objectData);
                    }
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