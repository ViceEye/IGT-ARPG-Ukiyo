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
        }

        public void UpdatePanel()
        {
            // Debug data viewer
            _itemDictionary.Clear();
            for (var i = 0; i < _inventoryModule.slotDataMap.Values.Count; i++)
                _itemDictionary.Add(_inventoryModule.slotDataMap.Values.ToArray()[i].ToString());
            
            foreach (var itemSlotData in _inventoryModule.slotDataMap.Values)
            {
                _gridObj.slotList[itemSlotData.SlotId].SetItem(itemSlotData.Item);
                _gridObj.slotList[itemSlotData.SlotId].active = true;
            }

            SaveInventoryData();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                if (_canvasGroup.alpha >= 1.0f)
                {
                    _canvasGroup.alpha = 0;
                    foreach (var slot in _gridObj.slotList.Values)
                    {
                        if (slot != null && slot.UIItem != null)
                        {
                            slot.active = false;
                            slot.toolTip.Hide();
                        }
                    }
                }
                else
                {
                    _canvasGroup.alpha = 1.0f;
                    foreach (var slot in _gridObj.slotList.Values)
                    {
                        if (slot != null && slot.UIItem != null)
                            slot.active = true;
                    }
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
            Debug.Log(source);
            _inventoryModule.AddItem(source);
        }

        public void Remove(int source)
        {
            _inventoryModule.RemoveItem(source);
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