using System;
using System.Collections.Generic;
using Ukiyo.Common;
using Ukiyo.UI.Slot;
using UnityEngine;

namespace Ukiyo.UI.Inventory
{
    /// <summary>
    /// View of inventory
    /// </summary>
    public class InventoryGrid : MonoBehaviour
    {
        public UIToolTip UIToolTip;
        public UIPickedItem UIPickedItem;
        public Dictionary<int, InventorySlot> slotList = new Dictionary<int, InventorySlot>();

        public void Init(int maxGridSize)
        {
            for (int i = 1; i <= maxGridSize; i++)
            {
                GameObject slotGo = Resources.Load<GameObject>(UIDefines.UI_Inventory_Slot);
                GameObject slot = Instantiate(slotGo, transform);   // Instantiate Slot Case
                InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
                inventorySlot.toolTip = UIToolTip;
                inventorySlot.pickedItem = UIPickedItem;
                inventorySlot.Init(i);
                slotList.Add(i, inventorySlot);
            }
        }
    }
}