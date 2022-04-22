using System.Collections.Generic;
using Ukiyo.Common.Object;
using Ukiyo.Serializable;
using Ukiyo.UI.Slot;

namespace Ukiyo.UI.Inventory
{
    public class InventoryModule
    {
        public Dictionary<int, InventorySlot> slotDataMap = new Dictionary<int, InventorySlot>();

        public void LoadInventoryData(Dictionary<int, ItemData> loadInventoryData)
        {
            
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

            InventorySlot[] slots = GetSameItemSlotsWithEmpty(itemData);  // Get same ID slots and empty slots

            int remainderAddAmount = amount;  // Declare a variable to store the remaining amount to add

            int index = 0;

            while (remainderAddAmount > 0)  // When the remaining amount to be added is greater than 0
            {
                if (slots[index].UIItem != null) // If the target slot is not an empty slot
                {
                    if (slots[index].UIItem.Space >= remainderAddAmount) // When the space of the blank slot is larger than the remaining number to be added
                    {
                        slots[index].UIItem.Amount += remainderAddAmount; // Add the remaining amount to the empty space
                        
                        remainderAddAmount = 0; // The amount has been added, set the remaining number to be zero, and end the adding process
                    }
                    else  // If the remaining amount to be added is greater than the grid space
                    {
                        remainderAddAmount -= slots[index].UIItem.Fill();  // Fill up the slots, the remaining amount minus the amount added by the slots
                    }
                }
                else  // When the target grid is empty
                {
                    // todo: Currently not possible to be null, and slots[index].UIItem.SetItem is invalid when its null. Need update resource center so GameObject can be load from ObjectPool so can be set through Slot
                    if (itemData.Capacity >= remainderAddAmount)  // If the item's single slot capacity is greater than or equal to the remaining number that needs to be added
                    {
                        slots[index].CreateItem(itemData);
                        slots[index].UIItem.Amount = remainderAddAmount;  // Directly set the item and quantity of the blank slot
                        
                        remainderAddAmount = 0;  // The amount has been added, set the remaining number to be zero, and end the adding process
                    }
                    else  //  When the capacity of an empty space cannot fully accommodate the amount that needs to be added
                    {
                        slots[index].CreateItem(itemData);
                        slots[index].UIItem.Amount = itemData.Capacity;  // Directly set the item and quantity of the blank slot
                        
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
                InventorySlot slot = GetSlotWithMinAmountOfItem(item);  // Get the slot in the backpack that has the item with the least number of items
                if (remainderAmount > slot.UIItem.Amount)  // If the remaining number to be removed is greater than the number stored in the target slot
                {
                    remainderAmount -= slot.SetEmpty();  // Clear the slot, the variable remaining Amount minus the amount that is decremented when the slot is cleared
                }
                else  // If the number stored in the target slot is greater than the remaining number to be removed
                {
                    slot.UIItem.Amount -= remainderAmount;  // The number of slot minus the remaining number to be removed
                    remainderAmount = 0;  // The removal operation is completed, and the remaining number to be removed is set to zero
                }

            }
            return true;
        }

        #endregion

        #region Swap Item

        public void SwapItem(int slotID1, int slotID2)
        {
            UIItemData uiItemData = slotDataMap[slotID1].UIItem;
            slotDataMap[slotID1].SetItem(slotDataMap[slotID2].UIItem);
            slotDataMap[slotID2].SetItem(uiItemData);
        }

        #endregion

        #region Functions

        // Get the one with the smallest number of items in all the slots occupied by an item
        public InventorySlot GetSlotWithMinAmountOfItem(ItemData item)
        {
            int minAmount = int.MaxValue;
            int minAmountIndex = -1;

            InventorySlot[] slots = GetSameItemSlotsWithoutEmpty(item);  // Get all slots with the same item, excluding empty slots

            List<InventorySlot> listSlots = new List<InventorySlot>(slots);

            for (int i = 0; i < listSlots.Count; i++)  // Calculate the minimum value and get the corresponding index
            {
                int amountInThisSlot = listSlots[i].UIItem.Amount;

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
        private InventorySlot[] GetSameItemSlotsWithoutEmpty(ItemData itemData)
        {
            List<InventorySlot> listSlots = new List<InventorySlot>();
            
            foreach (var slot in slotDataMap.Values)
            {
                if (slot.UIItem == null || slot.UIItem.Item.ID == itemData.ID)
                    listSlots.Add(slot);
            }

            return listSlots.ToArray();
        }
        
        // Get all the slots of the same item and the remaining empty slots
        private InventorySlot[] GetSameItemSlotsWithEmpty(ItemData itemData)
        {
            List<InventorySlot> listSlots = new List<InventorySlot>();

            foreach (var slot in slotDataMap.Values)
            {
                if (slot.UIItem == null || slot.UIItem.Item.ID == itemData.ID)
                    listSlots.Add(slot);
            }
            
            listSlots.Sort((x, y) =>
            {
                if (x.UIItem != null && y.UIItem != null)
                    return x.UIItem.Amount.CompareTo(y.UIItem.Amount) * -2 + x.SlotId.CompareTo(y.SlotId);
                return x.SlotId.CompareTo(y.SlotId);
            });
            
            return listSlots.ToArray();
        }

        #endregion
    }
}