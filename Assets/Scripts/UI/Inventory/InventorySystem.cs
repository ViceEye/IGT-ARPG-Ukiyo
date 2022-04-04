using System;
using System.Collections.Generic;
using Ukiyo.Common;
using Ukiyo.Serializable;
using UnityEngine;

namespace Ukiyo.UI.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        private Dictionary<ObjectData, ItemData> _itemDictionary;

        public List<ObjectData> _itemData;

        public List<ItemData> __inventoryItems;
        [NonSerialized]protected List<ItemData> _inventoryItems;
        public List<ItemData> InventoryItems { get => _inventoryItems; set => _inventoryItems = value; }

        [SerializeField]
        private EnumInventoryItemType _currentOpenedPanel;
        public EnumInventoryItemType CurrentOpenedPanel => _currentOpenedPanel;

        private void Awake()
        {
            InventoryItems = __inventoryItems;
            _itemDictionary = new Dictionary<ObjectData, ItemData>();
        }

        public void Add(ObjectData source)
        {
            if (_itemDictionary.TryGetValue(source, out ItemData value))
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
            if (_itemDictionary.TryGetValue(source, out ItemData value))
            {
                value.ReduceFromStack();

                if (value.stackSize == 0)
                {
                    InventoryItems.Remove(value);
                    _itemDictionary.Remove(source);
                }
                
            }
        }
    }
}