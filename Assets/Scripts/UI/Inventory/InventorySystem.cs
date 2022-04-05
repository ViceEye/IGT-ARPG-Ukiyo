using System;
using System.Collections.Generic;
using System.Linq;
using Ukiyo.Common;
using Ukiyo.Common.Object;
using Ukiyo.Serializable;
using Ukiyo.UI.Slot;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Ukiyo.UI.Inventory
{
    public class InventorySystem : MonoBehaviour
    {
        [Header("Inventory Data")]
        protected Grid gridObj;

        private Dictionary<int, ItemData> _itemDictionary;

        [Header("Panel Button Settings")]
        public Color openTagTextColor;
        public Color closeTagTextColor;
        public List<RectTransform> panelBtnList;
        public List<Text> panelBtnTextList;
        
        [SerializeField] private EnumInventoryItemType _currentOpenedPanel;
        public EnumInventoryItemType CurrentOpenedPanel => _currentOpenedPanel;

        [SerializeField] private float btnAnimationDuration = 0.2f;

        private void Awake()
        {
            // Pre Initialization
            ObjectPool.Instance.Init();
            DataSaver.Instance.Init();
            
            // Initialization
            _itemDictionary = DataSaver.Instance.LoadInventoryData();
            gridObj = GetComponentInChildren<Grid>();
            if (gridObj == null)
                Debug.LogWarning("Inventory System has no grid component");
        }

        private void Start()
        {
            UpdatePanel();
        }

        public void UpdatePanel()
        {
            var inventoryList = _itemDictionary.ToList();
            for (var i = 0; i < inventoryList.Count; i++)
            {
                UIItemData uiItemData = gridObj.slotList[i].gameObject.GetComponentInChildren<UIItemData>();
                if (uiItemData == null)
                {
                    GameObject item = Instantiate(gridObj.itemGO, gridObj.slotList[i].gameObject.transform);
                    uiItemData = item.GetComponent<UIItemData>();
                }
                uiItemData.SetItem(inventoryList[i].Value);
            }
        }

        private void FixedUpdate()
        {
            test = _itemDictionary.Values.ToList();
        }

        public void SaveInventoryData()
        {
            DataSaver.Instance.SaveInventoryData(_itemDictionary.Values.ToList());
        }

        public void TestAdd()
        {
            int random = new Random().Next(10000, 10010);
            Add(random);
        }

        public void Add(int source)
        {
            Debug.Log(source);
            if (_itemDictionary.TryGetValue(source, out ItemData value))
            {
                value.AddToStack();
            }
            else
            {
                ObjectData objData = ObjectPool.Instance.GetItemById(source);
                
                if (objData != null)
                {
                    ItemData newItemData = new ItemData(objData);
                    _itemDictionary.Add(source, newItemData);
                }
            }
        }

        public void Remove(int source)
        {
            if (_itemDictionary.TryGetValue(source, out ItemData value))
            {
                value.ReduceFromStack();

                if (value.stackSize == 0)
                {
                    _itemDictionary.Remove(source);
                }
            }
        }

        public void SwitchPanel()
        {
            switch (CurrentOpenedPanel)
            {
                case EnumInventoryItemType.Equipment:
                {
                    // Move out Consumable button and change font color
                    StartCoroutine(Utils.MoveY(panelBtnList[(int) EnumInventoryItemType.Consumable], 20, btnAnimationDuration));
                    panelBtnTextList[(int) EnumInventoryItemType.Consumable].color = openTagTextColor;
                    
                    // Move in Equipment button and change font color
                    StartCoroutine(Utils.MoveY(panelBtnList[(int) EnumInventoryItemType.Equipment], 0, btnAnimationDuration));
                    panelBtnTextList[(int) EnumInventoryItemType.Equipment].color = closeTagTextColor;
                    
                    _currentOpenedPanel = EnumInventoryItemType.Consumable;
                    break;
                }
                case EnumInventoryItemType.Consumable:
                {
                    // Move out Equipment button and change font color
                    StartCoroutine(Utils.MoveY(panelBtnList[(int) EnumInventoryItemType.Equipment], 20, btnAnimationDuration));
                    panelBtnTextList[(int) EnumInventoryItemType.Equipment].color = openTagTextColor;
                    
                    // Move in Consumable button and change font color
                    StartCoroutine(Utils.MoveY(panelBtnList[(int) EnumInventoryItemType.Consumable], 0, btnAnimationDuration));
                    panelBtnTextList[(int) EnumInventoryItemType.Consumable].color = closeTagTextColor;
                    
                    _currentOpenedPanel = EnumInventoryItemType.Equipment;
                    break;
                }
            }
        }
    }
}