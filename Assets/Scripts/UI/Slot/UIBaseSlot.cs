using Ukiyo.Common;
using Ukiyo.Serializable;
using Ukiyo.UI.Inventory;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ukiyo.UI.Slot
{
    public abstract class UIBaseSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
    {
        public Color empty;
        public Color hover;
        public float fadeSpeed;
        
        public UIToolTip toolTip;
        public UIPickedItem pickedItem;

        protected Image image;

        [SerializeField]
        protected UIItemData uiItem;
        public UIItemData UIItem
        {
            get => uiItem;
            set => uiItem = value;
        }

        [SerializeField]
        protected int slotID;
        public int SlotId => slotID;
        public bool active = true;
        public bool allowPickup = true;
        
        public virtual void Init(int id)
        {
            slotID = id;
            name = "Slot-" + id;
        }

        public virtual void SetItem(ItemData itemData)
        {
            if (itemData == null)
            {
                SetEmpty();
                return;
            }
            
            if (uiItem == null)
            {
                GameObject itemGo = Resources.Load<GameObject>(PrefabDefines.UI_INVENTORY_ITEM);
                GameObject item = Instantiate(itemGo, transform);
                item.SetActive(true);
                UIItemData uiItemData = item.GetComponent<UIItemData>();
                uiItemData.Init();
                UIItem = uiItemData;
            }
            
            UIItem.SetItem(itemData);
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
            StartCoroutine(Utils.Coloring(image, hover, fadeSpeed, 0));

            if (UIItem != null && active)
            {
                toolTip.Show(this, transform.position);   
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            StartCoroutine(Utils.Coloring(image, empty, fadeSpeed, 0));
            
            if (UIItem != null)
            {
                toolTip.Hide();   
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (UIItem != null && active && allowPickup && eventData.button == PointerEventData.InputButton.Right)
            {
                pickedItem.PickUpItem(this);
                Hide();
            }
        }

        private void Awake()
        {
            image = GetComponent<Image>();
            uiItem = GetComponentInChildren<UIItemData>();
        }

        protected virtual void Hide()
        {
            if (UIItem != null)
                UIItem.gameObject.SetActive(false);
            active = false;
        }

        public virtual void Show()
        {
            if (UIItem != null)
                UIItem.gameObject.SetActive(true);
            active = true;
        }
        
        public override string ToString()
        {
            return SlotId + ": " + (UIItem == null ? "null" : UIItem.item);
        }
    }
}