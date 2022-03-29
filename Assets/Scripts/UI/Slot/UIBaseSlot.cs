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
        [SerializeField]
        protected int slotID;
        public int SlotId => slotID;

        public void Init(int id)
        {
            slotID = id;
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
    }
}