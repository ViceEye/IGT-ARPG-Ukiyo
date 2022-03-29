using System;
using Ukiyo.Serializable;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using UnityEngine.UI;

namespace Ukiyo.UI.Slot
{
    /// <summary>
    /// Item for ui
    /// </summary>
    public class UIItemData : MonoBehaviour
    {
        public ObjectData debugData;
        public ItemData Item { get; private set; }

        protected Image image;
        protected Image cdMask;
        protected Text stack;

        [SerializeField]
        protected string item;
        [SerializeField] 
        protected int maxStackSize;

        public void SetItem(ItemData itemData)
        {
            Item = itemData;
            image.sprite = itemData.data.Icon;
        }

        public bool AddStack()
        {
            // todo: Create another data
            if (Item.stackSize == maxStackSize) return false;
            
            Item.AddToStack();
            stack.text = Item.stackSize == 1 ? "" : Item.stackSize.ToString();
            return true;
        }
        
        public bool Reduce()
        {
            // todo: Search another data
            if (Item.stackSize == 0) return false;
            
            Item.ReduceFromStack();
            stack.text = Item.stackSize == 1 ? "" : Item.stackSize.ToString();
            return true;
        }

        public bool Set(int amount)
        {
            // Validation
            if (amount <= 0 || amount > maxStackSize) return false;
            
            Item.SetTheStack(amount);
            stack.text = Item.stackSize == 1 ? "" : Item.stackSize.ToString();
            return true;
        }
        

        private void Awake()
        {
            image = GetComponent<Image>();
            cdMask = transform.Find("CD").GetComponent<Image>();
            stack = GetComponentInChildren<Text>();

            Item = new ItemData(debugData);
            item = JsonUtility.ToJson(debugData);
            Debug.Log(JsonUtility.ToJson(debugData));
        }

        private void OnDestroy()
        {
            image.sprite = null;
            stack.text = "";
            
        }
    }
}