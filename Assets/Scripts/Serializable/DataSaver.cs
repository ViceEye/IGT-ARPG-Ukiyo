using System;
using System.Collections.Generic;
using System.Linq;
using LitJson;
using Ukiyo.Common;
using Ukiyo.Common.Object;
using Ukiyo.Common.Singleton;
using Ukiyo.UI.Inventory;
using Ukiyo.UI.Slot;
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
        
        // Save Inventory Data
        public void SaveInventoryData(List<ItemSlotData> value)
        {
            List<InventoryJsonData> inventoryJsonDataList = new List<InventoryJsonData>();
            
            foreach (var slot in value)
            {
                if (slot.Item != null)
                    inventoryJsonDataList.Add(new InventoryJsonData(slot.SlotId, slot.Item));
            }
            
            Utils.WriteIntoFile(inventoryJsonDataList, savaDataFilePath, inventoryFileName);
        }

        // Load Inventory Data
        public Dictionary<int, ItemData> LoadInventoryData()
        {
            Dictionary<int, ItemData> inventoryItemDataList = new Dictionary<int, ItemData>();

            string inventoryJson = Utils.GetJsonStr(savaDataFilePath, inventoryFileName);

            if (inventoryJson != "" && inventoryJson != "[{}]")
            {
                JsonData inventoryData = JsonMapper.ToObject(inventoryJson);

                // Convert JsonData to Object
                foreach (JsonData jsonData in inventoryData)
                {
                    int slotId = int.Parse(jsonData["slotId"].ToString());
                    int id = int.Parse(jsonData["itemId"].ToString());
                    int stack = int.Parse(jsonData["stack"].ToString());

                    ObjectData objData = ObjectPool.Instance.GetItemById(id);
                    if (objData != null)
                    {
                        ItemData data = new ItemData(objData, stack);
                        inventoryItemDataList.Add(slotId, data);
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
            itemId = itemData.ID;
            stack = itemData._stack;
        }
    }
}