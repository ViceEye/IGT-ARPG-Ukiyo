using System;
using System.Collections.Generic;
using Ukiyo.UI.Slot;
using UnityEngine;

namespace Ukiyo.UI.Inventory
{
    public class InventoryGrid : MonoBehaviour
    {
        public GameObject slotGO;
        
        public int __maxGridSize;
        [NonSerialized]protected int _maxGridSize;
        public int MaxGridSize { get => _maxGridSize; set => _maxGridSize = value; }

        public Dictionary<int, InventorySlot> Init()
        {
            Dictionary<int, InventorySlot> slotList = new Dictionary<int, InventorySlot>();
            MaxGridSize = __maxGridSize;
            for (int i = 1; i <= MaxGridSize; i++)
            {
                GameObject slot = Instantiate(slotGO, transform);   // Instantiate Slot Case
                InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                
                slotList.Add(i, inventorySlot);
            }
            return slotList;
        }
    }
}