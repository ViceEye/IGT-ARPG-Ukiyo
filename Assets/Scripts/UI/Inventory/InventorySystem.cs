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
        // Inventory runtime data
        [Header("Inventory Data")]
        protected InventoryGrid _gridObj;
        public List<string> _itemDictionary;
        private CanvasGroup _canvasGroup;

        // Panel Button Settings
        [Header("Panel Button Settings")]
        public Color openTagTextColor;
        public Color closeTagTextColor;
        public List<RectTransform> panelBtnList;
        public List<Text> panelBtnTextList;
        
        [SerializeField] private EnumInventoryItemType _currentOpenedPanel;
        public EnumInventoryItemType CurrentOpenedPanel => _currentOpenedPanel;
        [SerializeField] private float btnAnimationDuration = 0.2f;

        protected InventoryModule inventoryModule;
        protected int _maxGridSize = 64;
        
        private void Awake()
        {
            // Pre Initialization
            ObjectPool.Instance.Init();
            DataSaver.Instance.Init();
            _itemDictionary = new List<string>();
            
            // Initialization
            _gridObj = GetComponentInChildren<InventoryGrid>();
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_gridObj == null)
                Debug.LogWarning("Inventory System has no grid component");
            
            inventoryModule = new InventoryModule { slotDataMap = _gridObj.Init() };
            for (var i = 0; i < inventoryModule.slotDataMap.Values.Count; i++)
            {
                _itemDictionary.Add(inventoryModule.slotDataMap.Values.ToArray()[i].ToString());
            }
            inventoryModule.LoadInventoryData(DataSaver.Instance.LoadInventoryData(inventoryModule));
        }

        private void Start()
        {
            UpdatePanel();
        }

        public void UpdatePanel()
        {
            for (var i = 0; i < inventoryModule.slotDataMap.Values.Count; i++)
            {
                _itemDictionary.Add(inventoryModule.slotDataMap.Values.ToArray()[i].ToString());
            }
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.E))
            {
                _canvasGroup.alpha = _canvasGroup.alpha >= 1.0f ? 0.0f : 1.0f;
            }
        }

        public void SaveInventoryData()
        {
            
        }

        public void TestAdd()
        {
            int random = new Random().Next(10001, 10010);
            Add(random);
        }

        public void Add(int source)
        {
            Debug.Log(source);
            inventoryModule.AddItem(source);
        }

        public void Remove(int source)
        {
            inventoryModule.RemoveItem(source);
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