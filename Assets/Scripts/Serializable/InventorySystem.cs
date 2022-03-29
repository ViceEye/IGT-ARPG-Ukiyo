using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ukiyo.Serializable
{
    public class InventorySystem : MonoBehaviour
    {
        private Dictionary<ObjectData, ItemData> m_itemDictionary;

        private int maxInventorySize = 64;

        public List<ObjectData> itemData;

        public List<ItemData> __inventoryItems;
        [NonSerialized]protected List<ItemData> _inventoryItems;
        public List<ItemData> InventoryItems { get => _inventoryItems; set => _inventoryItems = value; }

        private void Awake()
        {
            InventoryItems = __inventoryItems;
            m_itemDictionary = new Dictionary<ObjectData, ItemData>();
        }

        public void Add(ObjectData source)
        {
            if (m_itemDictionary.TryGetValue(source, out ItemData value))
            {
                value.AddToStack();
            }
            else
            {
                ItemData newItemData = new ItemData(source);
                InventoryItems.Add(newItemData);
            }
        }

        public void Remove(ObjectData source)
        {
            if (m_itemDictionary.TryGetValue(source, out ItemData value))
            {
                value.ReduceFromStack();

                if (value.stackSize == 0)
                {
                    InventoryItems.Remove(value);
                    m_itemDictionary.Remove(source);
                }
                
            }
        }
    }
}