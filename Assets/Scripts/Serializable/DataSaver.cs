using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using Ukiyo.Common;
using Ukiyo.Common.Object;
using Ukiyo.Common.Singleton;
using UnityEngine;

namespace Ukiyo.Serializable
{
    public class DataSaver : MonoSingleton<DataSaver>, IObject
    {
        
        private string savaDataFilePath = "/Resources/SaveData/";
        private string inventoryFileName = "PlayerInventory.json";

        public void Init()
        {
        }

        public void OnSpawn()
        {
        }

        public void OnDespawn()
        {
        }
        
        public void SaveInventoryData(List<ItemData> value)
        {
            List<InventoryJsonData> inventoryJsonDataList = new List<InventoryJsonData>();
            
            foreach (var itemData in value)
            {
                inventoryJsonDataList.Add(new InventoryJsonData(1, itemData));
            }
            
            Utils.WriteIntoFile(inventoryJsonDataList, savaDataFilePath, inventoryFileName);
        }

        [ContextMenu("LoadInventoryData")]
        public Dictionary<int, ItemData> LoadInventoryData()
        {
            Dictionary<int, ItemData> inventoryItemDataList = new Dictionary<int, ItemData>();

            string inventoryJson = Utils.GetJsonStr(savaDataFilePath, inventoryFileName);

            if (inventoryJson != "" && inventoryJson != "[{}]")
            {
                JsonData inventoryData = JsonMapper.ToObject(inventoryJson);

                foreach (JsonData jsonData in inventoryData)
                {
                    int id = int.Parse(jsonData["itemId"].ToString());
                    int stack = int.Parse(jsonData["stack"].ToString());

                    ObjectData objData = ObjectPool.Instance.GetItemById(id);
                    if (objData != null)
                    {
                        ItemData data = new ItemData(objData, stack);
                        inventoryItemDataList.Add(data.data.ID, data);
                    }
                }
            }

            return inventoryItemDataList;
        }
    }

    [Serializable]
    class InventoryJsonData
    {
        public int slotId { get; set; }
        public int itemId { get; set; }
        public int stack { get; set; }

        public InventoryJsonData(int slot, ItemData itemData)
        {
            slotId = slot;
            itemId = itemData.data.ID;
            stack = itemData.stackSize;
        }
    }
}