using System;
using System.Collections.Generic;
using Ukiyo.UI.Slot;
using UnityEngine;

namespace Ukiyo.UI.Inventory
{
    public class Grid : MonoBehaviour
    {
        public GameObject slotGO;
        public GameObject itemGO;
        public List<InventorySlot> slotList;
        
        public int __maxGridSize;
        [NonSerialized]protected int _maxGridSize;
        public int MaxGridSize { get => _maxGridSize; set => _maxGridSize = value; }

        private void Awake()
        {
            MaxGridSize = __maxGridSize;
            for (int i = 1; i <= MaxGridSize; i++)
            {
                GameObject slot = Instantiate(slotGO, transform);
                InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                inventorySlot.Init(i);
                slotList.Add(inventorySlot);
            }
        }
    }
}