using System;
using Ukiyo.Common;
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
        public ItemData Item { get; private set; }

        protected Image image;
        protected Image cdMask;
        protected Text stack;

        public string item; // Debug data

        public void SetItem(ItemData itemData)
        {
            Item = itemData;
            image.sprite = itemData.Icon;
            
            item = Item._stack + " × " + JsonUtility.ToJson(Item);
            UpdateStackText();
        }

        public void SetEmpty()
        {
            Item = null;
            image = null;  // Change back to transparent image

            if (stack != null) stack.text = "";
            item = "";
        }

        #region Capacity Modification
        public int Space => Item.Capacity - Item._stack;
        public int Amount
        {
            get
            {
                if (Item == null)
                {
                    return 0;
                }
                return Item._stack;
            }
            set
            {
                Item.SetStack(value);
                UpdateStackText();

                if (Item._stack <= 0) SetEmpty();
            }
        }
        public int Fill()
        {
            int lastAmount = Amount;
            Amount = Item.Capacity;
            return Amount - lastAmount;
        }
        #endregion

        public void Init()
        {
            image = GetComponent<Image>();
            cdMask = transform.Find("CD").GetComponent<Image>();
            stack = GetComponentInChildren<Text>();
        }

        public void UpdateStackText()
        {
            stack.text = Item._stack == 1 ? "" : Item._stack.ToString();
        }
    }
}