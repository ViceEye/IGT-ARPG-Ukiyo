using System;
using System.Collections.Generic;
using System.Linq;
using Ukiyo.Common;
using Ukiyo.Common.Object;
using Ukiyo.Serializable;
using UnityEngine;

namespace Ukiyo.UI.Inventory
{
    public class InventoryModule
    {
        public int _maxGridSize;
        public Dictionary<int, ItemSlotData> slotDataMap = new Dictionary<int, ItemSlotData>();

        public void LoadInventoryData()
        {
            slotDataMap.Clear();
            var inventoryData = DataSaver.Instance.LoadInventoryData();
            for (int i = 1; i <= _maxGridSize; i++)
            {
                slotDataMap.Add(i, new ItemSlotData(i));
            }
            foreach (var keyValue in inventoryData)
            {
                int slot = keyValue.Key;
                ItemData item = keyValue.Value;
                if (item != null)
                    slotDataMap[slot].Item = item;
            }
        }

        #region Add Item

        
        public bool AddItem(int itemId, int amount = 1)
        {
            return AddItem(new ItemData(ObjectPool.Instance.GetItemById(itemId)), amount);
        }

        public bool AddItem(ItemData itemData, int amount = 1)
        {
            if (itemData == null)
                return false;

            ItemSlotData[] slots = GetSameItemSlotsWithEmpty(itemData);  // Get same ID slots and empty slots

            int remainderAddAmount = amount;  // Declare a variable to store the remaining amount to add

            int index = 0;

            while (remainderAddAmount > 0)  // When the remaining amount to be added is greater than 0
            {
                if (slots[index].Item != null) // If the target slot is not an empty slot
                {
                    if (slots[index].Space >= remainderAddAmount) // When the space of the blank slot is larger than the remaining number to be added
                    {
                        slots[index].Amount += remainderAddAmount; // Add the remaining amount to the empty space
                        
                        remainderAddAmount = 0; // The amount has been added, set the remaining number to be zero, and end the adding process
                    }
                    else  // If the remaining amount to be added is greater than the grid space
                    {
                        remainderAddAmount -= slots[index].Fill();  // Fill up the slots, the remaining amount minus the amount added by the slots
                    }
                }
                else  // When the target grid is empty
                {
                    // todo: Currently not possible to be null, and slots[index].UIItem.SetItem is invalid when its null. Need update resource center so GameObject can be load from ObjectPool so can be set through Slot
                    if (itemData.Capacity >= remainderAddAmount)  // If the item's single slot capacity is greater than or equal to the remaining number that needs to be added
                    {
                        slots[index].Item = itemData;
                        slots[index].Amount = remainderAddAmount;  // Directly set the item and quantity of the blank slot
                        
                        remainderAddAmount = 0;  // The amount has been added, set the remaining number to be zero, and end the adding process
                    }
                    else  //  When the capacity of an empty space cannot fully accommodate the amount that needs to be added
                    {
                        slots[index].Item = itemData;
                        slots[index].Amount = itemData.Capacity;  // Directly set the item and quantity of the blank slot
                        
                        remainderAddAmount -= itemData.Capacity; //  The remaining number to be added minus the capacity of the blank space
                    }

                }

                index++;
            }

            return true;
        }

        #endregion

        #region Remove Item

        public bool RemoveItem(int itemID, int amount = 1)
        {
            return RemoveItem(new ItemData(ObjectPool.Instance.GetItemById(itemID)), amount);
        }

        public bool RemoveItem(ItemData item, int amount = 1)
        {

            int remainderAmount = amount;  // Record the remaining number to be removed, initialized to the total number to be removed

            while (remainderAmount > 0)
            {
                ItemSlotData slot = GetSlotWithMinAmountOfItem(item);  // Get the slot in the backpack that has the item with the least number of items
                if (remainderAmount > slot.Amount)  // If the remaining number to be removed is greater than the number stored in the target slot
                {
                    remainderAmount -= slot.Clear();  // Clear the slot, the variable remaining Amount minus the amount that is decremented when the slot is cleared
                }
                else  // If the number stored in the target slot is greater than the remaining number to be removed
                {
                    slot.Amount -= remainderAmount;  // The number of slot minus the remaining number to be removed
                    remainderAmount = 0;  // The removal operation is completed, and the remaining number to be removed is set to zero
                }

            }
            return true;
        }

        #endregion

        #region Swap Item

        public void SwapItem(int slotID1, int slotID2)
        {
            (slotDataMap[slotID1].Item, slotDataMap[slotID2].Item) = (slotDataMap[slotID2].Item, slotDataMap[slotID1].Item);
        }

        #endregion

        #region Functions
        
        // Sort all slot by id
        public ItemSlotData[] SortAllItemsById()
        {
            List<ItemSlotData> listSlots = new List<ItemSlotData>();
            
            foreach (var itemSlotData in slotDataMap.Values)
                if (itemSlotData.Item != null)
                    listSlots.Add(itemSlotData);
            
            listSlots.Sort((x, y) => x.Item.ID.CompareTo(y.Item.ID));
            slotDataMap.Clear();
            
            for (var i = 1; i <= _maxGridSize; i++)
            {
                if (i <= listSlots.Count)
                {
                    listSlots[i - 1].SlotId = i;
                    slotDataMap.Add(i, listSlots[i - 1]);
                    continue;
                }
                slotDataMap.Add(i, new ItemSlotData(i));
            }
            
            return listSlots.ToArray();
        }
        
        // Get all slots of the same type
        public ItemSlotData[] GetAllItemSlotsWithType(EnumInventoryItemType type)
        {
            List<ItemSlotData> listSlots = new List<ItemSlotData>();
            switch (type)
            {
                case EnumInventoryItemType.Content:
                {
                    return slotDataMap.Values.ToArray();
                }
                case EnumInventoryItemType.Equipment:
                case EnumInventoryItemType.Consumable:
                {
            
                    foreach (var slot in slotDataMap.Values)
                    {
                        if (slot.Item != null && slot.Item.Type == type)
                            listSlots.Add(slot);
                    }
                    break;
                }
            }
            
            listSlots.Sort((x, y) => x.Item.ID.CompareTo(y.Item.ID));
            
            return listSlots.ToArray();
        }

        // Get the one with the smallest number of items in all the slots occupied by an item
        public ItemSlotData GetSlotWithMinAmountOfItem(ItemData item)
        {
            int minAmount = int.MaxValue;
            int minAmountIndex = -1;

            ItemSlotData[] slots = GetSameItemSlotsWithoutEmpty(item);  // Get all slots with the same item, excluding empty slots

            List<ItemSlotData> listSlots = new List<ItemSlotData>(slots);

            for (int i = 0; i < listSlots.Count; i++)  // Calculate the minimum value and get the corresponding index
            {
                int amountInThisSlot = listSlots[i].Amount;

                if (minAmount > amountInThisSlot)
                {
                    minAmount = amountInThisSlot;
                    minAmountIndex = i;
                }
            }

            if (minAmountIndex == -1)  // Returns null if there is no such item
                return null;

            return listSlots[minAmountIndex];
        }
        
        // Get all slots of the same item
        private ItemSlotData[] GetSameItemSlotsWithoutEmpty(ItemData itemData)
        {
            List<ItemSlotData> listSlots = new List<ItemSlotData>();
            
            foreach (var slot in slotDataMap.Values)
            {
                if (slot.Item != null || slot.Item.ID == itemData.ID)
                    listSlots.Add(slot);
            }

            return listSlots.ToArray();
        }
        
        // Get all the slots of the same item and the remaining empty slots
        private ItemSlotData[] GetSameItemSlotsWithEmpty(ItemData itemData)
        {
            List<ItemSlotData> listSlots = new List<ItemSlotData>();

            foreach (var slot in slotDataMap.Values)
            {
                if (slot.Item == null || slot.Item.ID == itemData.ID)
                    listSlots.Add(slot);
            }
            
            listSlots.Sort((x, y) =>
            {
                if (x.Item != null && y.Item != null)
                    return x.Amount.CompareTo(y.Amount) * -2 + x.SlotId.CompareTo(y.SlotId);
                return x.SlotId.CompareTo(y.SlotId);
            });
            
            return listSlots.ToArray();
        }

        #endregion
    }

    /// <summary>
    /// Encapsulation of slot => item for runtime inventory storage in memory
    /// Separate control and view, avoid control to view directly
    /// </summary>
    [Serializable]
    public class ItemSlotData
    {
        [SerializeField]
        protected int slotId;
        public int SlotId
        {
            get => slotId;
            set => slotId = value;
        }

        [SerializeField]
        protected ItemData item;
        public ItemData Item
        {
            get => item;
            set => item = value;
        }

        public ItemSlotData(int slotId)
        {
            SlotId = slotId;
            Item = null;
        }

        public ItemSlotData(int slotId, ItemData item)
        {
            SlotId = slotId;
            Item = item;
        }

        #region Capacity Modification
        public int Space => Item.Capacity - Item._stack;
        
        public int Amount
        {
            get => Item?._stack ?? 0;
            set => Item.SetStack(value);
        }
        
        public int Fill()
        {
            int lastAmount = Amount;
            Amount = Item.Capacity;
            
            return Amount - lastAmount;
        }
        
        public int Clear()
        {
            item = null;
            int lastAmount = Amount;
            Amount = 0;

            return lastAmount;
        }
        #endregion

        public override string ToString()
        {
            if (Item != null)
                return SlotId + " - " + Item._stack + " × " + JsonUtility.ToJson(Item);
            return SlotId + " - Empty";
        }
    }
}