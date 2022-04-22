using System;
using Ukiyo.Common;
using Ukiyo.Serializable;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ukiyo.UI.Slot
{
    public abstract class UIBaseSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public Color empty;
        public Color hover;
        public float fadeSpeed;

        protected Image image;

        [SerializeField]
        protected UIItemData uiItem;
        public UIItemData UIItem => uiItem;
        
        [SerializeField]
        protected int slotID;
        public int SlotId => slotID;

        public virtual void Init(int id)
        {
            slotID = id;
            name = "Slot-" + id;
        }

        public virtual void CreateItem(ItemData itemData)
        {
            GameObject itemGo = Resources.Load<GameObject>("UI Prefabs/Item");
            GameObject item = Instantiate(itemGo, transform);
            item.SetActive(true);
            UIItemData uiItemData = item.GetComponent<UIItemData>();
            uiItemData.Init();
            uiItemData.SetItem(itemData);

            SetItem(uiItemData);
        }
        
        public virtual void SetItem(UIItemData item)
        {
            if (item == null)
            {
                SetEmpty();
                return;
            }

            uiItem = item;
        }

        public virtual int SetEmpty()
        {
            if (uiItem == null) return 0;
            int tempAmount = UIItem.Amount;
            uiItem.SetEmpty();
            uiItem = null;
            return tempAmount;
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            StartCoroutine(Utils.Coloring(image, hover, fadeSpeed));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StartCoroutine(Utils.Coloring(image, empty, fadeSpeed));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            
        }

        private void Awake()
        {
            image = GetComponent<Image>();
            uiItem = GetComponentInChildren<UIItemData>();
        }

        public override string ToString()
        {
            return SlotId + ": " + (UIItem == null ? "null" : UIItem.item);
        }
    }
}