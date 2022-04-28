using System.Collections.Generic;
using System.Linq;
using Ukiyo.Common;
using Ukiyo.Common.Object;
using Ukiyo.Serializable;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

namespace Ukiyo.UI.Inventory
{
    /// <summary>
    /// Manager of controller(InventoryModule) and view(InventoryGrid)
    /// </summary>
    public class InventorySystem : MonoBehaviour
    {
        // Inventory runtime data
        [Header("Inventory Debug Data")]
        public List<string> _itemDictionary;
        
        protected InventoryGrid _gridObj;
        private CanvasGroup _canvasGroup;

        // Panel Button Settings
        [Header("Panel Button Settings")]
        public Color openTagTextColor;
        public Color closeTagTextColor;
        public List<RectTransform> panelBtnList;
        public List<Text> panelBtnTextList;
        
        [Header("Inventory Settings")]
        [SerializeField]
        protected int _maxGridSize = 64;
        public int MaxGridSize { get => _maxGridSize; set => _maxGridSize = value; }
        
        [SerializeField] private EnumInventoryItemType _currentOpenedPanel;
        public EnumInventoryItemType CurrentOpenedPanel => _currentOpenedPanel;
        [SerializeField] private float btnAnimationDuration = 0.2f;

        /// <summary>
        /// Inventory Module holds CRUD functions
        /// </summary>
        protected InventoryModule _inventoryModule;
        public InventoryModule InventoryModule => _inventoryModule;
        
        private void Awake()
        {
            // Pre Initialization
            ObjectPool.Instance.Init();
            DataSaver.Instance.Init();
            _itemDictionary = new List<string>();
            
            // Register Components
            _gridObj = GetComponentInChildren<InventoryGrid>();
            _canvasGroup = GetComponent<CanvasGroup>();
            
            // Initialization
            if (_gridObj == null)
                Debug.LogWarning("Inventory System has no grid component");
            _gridObj.Init(MaxGridSize);

            _inventoryModule = new InventoryModule { _maxGridSize = MaxGridSize };

            // Debug content
            for (var i = 0; i < _inventoryModule.slotDataMap.Values.Count; i++)
                _itemDictionary.Add(_inventoryModule.slotDataMap.Values.ToArray()[i].ToString());
            
            _inventoryModule.LoadInventoryData();
        }

        private void Start()
        {
            UpdatePanel();
            DeactivateAllItems();
        }

        public void UpdatePanel()
        {
            UpdatePanel(_currentOpenedPanel);
        }

        private void UpdatePanel(EnumInventoryItemType type)
        {
            DeactivateAllItems();

            switch (type)
            {
                case EnumInventoryItemType.Content:
                {
                    _itemDictionary.Clear();
                    foreach (var itemSlotData in _inventoryModule.slotDataMap.Values)
                    {
                        _itemDictionary.Add(itemSlotData.ToString());
                        _gridObj.slotList[itemSlotData.SlotId].SetItem(itemSlotData.Item);
                        if (_gridObj.slotList[itemSlotData.SlotId].UIItem != null)
                            _gridObj.slotList[itemSlotData.SlotId].UIItem.gameObject.SetActive(true);
                        _gridObj.slotList[itemSlotData.SlotId].active = true;
                        _gridObj.slotList[itemSlotData.SlotId].allowPickup = true;
                    }
                    break;
                }
                case EnumInventoryItemType.Equipment:
                case EnumInventoryItemType.Consumable:
                {
                    int index = 1;
                    Debug.Log(index);
                    _itemDictionary.Clear();
                    foreach (var itemSlotData in _inventoryModule.GetAllItemSlotsWithType(type))
                    {
                        _itemDictionary.Add(itemSlotData.ToString());
                        _gridObj.slotList[index].SetItem(itemSlotData.Item);
                        _gridObj.slotList[index].UIItem.gameObject.SetActive(true);
                        _gridObj.slotList[index].active = true;
                        _gridObj.slotList[itemSlotData.SlotId].allowPickup = false;
                        index++;
                    }
                    break;
                }
            }
            SaveInventoryData();
        }

        protected void SwitchPanel(int type)
        {
            if (type > 2) return;
            EnumInventoryItemType itemType = (EnumInventoryItemType) type;
            if (_currentOpenedPanel == itemType)
                return;
            
            // Move in current button and change font color
            StartCoroutine(Utils.MoveY(panelBtnList[(int) _currentOpenedPanel], 0, btnAnimationDuration, 0));
            panelBtnTextList[(int) _currentOpenedPanel].color = closeTagTextColor;
            
            // Move out Consumable button and change font color
            StartCoroutine(Utils.MoveY(panelBtnList[type], 20, btnAnimationDuration, 0));
            panelBtnTextList[type].color = openTagTextColor;
            
            _currentOpenedPanel = (EnumInventoryItemType) type;
            UpdatePanel();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (_canvasGroup.alpha >= 1.0f)
                    _canvasGroup.alpha = 0;
                else
                {
                    _canvasGroup.alpha = 1.0f;
                    UpdatePanel();
                }
            }
        }

        public void SaveInventoryData()
        {
            DataSaver.Instance.SaveInventoryData(_inventoryModule.slotDataMap.Values.ToList());
        }

        public void TestAdd()
        {
            int random = new Random().Next(10001, 10010);
            Add(random);
        }

        public void Add(int source)
        {
            _inventoryModule.AddItem(source);
        }

        public void Remove(int source)
        {
            _inventoryModule.RemoveItem(source);
        }

        public void Sort()
        {
            _inventoryModule.SortAllItemsById();
            UpdatePanel();
        }

        private void DeactivateAllItems()
        {
            foreach (var slot in _gridObj.slotList.Values)
            {
                if (slot != null && slot.UIItem != null)
                {
                    slot.active = false;
                    slot.allowPickup = false;
                    
                    slot.toolTip.Hide();
                    slot.UIItem.gameObject.SetActive(false);
                }
            }
        }
    }
}