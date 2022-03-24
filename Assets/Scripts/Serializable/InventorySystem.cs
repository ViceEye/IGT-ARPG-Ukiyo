using System;
using System.Collections.Generic;
using UnityEngine;

namespace Ukiyo.Serializable
{
    public class InventorySystem : MonoBehaviour
    {
        private Dictionary<ObjectData, ItemData> m_itemDictionary;
        public List<ItemData> inventoryItems { get; private set; }

        private void Awake()
        {
            m_itemDictionary = new Dictionary<ObjectData, ItemData>();
            inventoryItems = new List<ItemData>();
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
                inventoryItems.Add(newItemData);
            }
        }

        public void Remove(ObjectData source)
        {
            if (m_itemDictionary.TryGetValue(source, out ItemData value))
            {
                value.ReduceFromStack();

                if (value.stackSize == 0)
                {
                    inventoryItems.Remove(value);
                    m_itemDictionary.Remove(source);
                }
                
            }
        }
    }
}