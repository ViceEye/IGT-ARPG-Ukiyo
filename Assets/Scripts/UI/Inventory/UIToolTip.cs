using System.Collections.Generic;
using Ukiyo.Serializable;
using Ukiyo.UI.Slot;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ukiyo.UI.Inventory
{
    public class UIToolTip : MonoBehaviour
    {
        public GraphicRaycaster graphicRaycaster;
        public Image image;
        public Text nameText;
        public Text tooltipText;
        public Text contentText;

        public float leftSidePivotX;
        public float rightSidePivotX;
        public float upsidePivotY;
        public float downSidePivotY;

        private RectTransform rectTransform;
        private EventSystem eventSystem;
        private CanvasGroup canvasGroup;
        private UIAnimation uiAnimation;

        private bool isShow = false;
        public bool IsShow => isShow;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            canvasGroup = GetComponent<CanvasGroup>();
            uiAnimation = GetComponent<UIAnimation>();
            eventSystem = EventSystem.current;
        }

        public void Start()
        {
            Show();
        }

        private void Update()
        {
            if (IsShow)
            {
                PointerEventData eventData = new PointerEventData(eventSystem);
                eventData.pressPosition = Input.mousePosition;
                eventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                graphicRaycaster.Raycast(eventData, results);

                bool isHitSlot = false;
                UIBaseSlot slot = null;
                
                foreach (var raycastResult in results)
                {
                    slot = raycastResult.gameObject.GetComponent<UIBaseSlot>();
                    if (slot != null)
                    {
                        isHitSlot = true;
                        break;
                    }
                }

                if (isHitSlot && slot.UIItem == null)
                    Hide();

                if (!isHitSlot)
                    Hide();
            }
        }
        
        public void Show(UIBaseSlot slot, Vector2 pos)
        {
            SetTooltip(slot);
            transform.position = pos;
            transform.SetAsLastSibling();

            Vector2 pivotPos = Vector2.one;

            pivotPos = pos.x > Screen.width / 2 ? new Vector2(rightSidePivotX, pivotPos.y) : new Vector2(leftSidePivotX, pivotPos.y);

            pivotPos = pos.y > Screen.height / 2 ? new Vector2(pivotPos.x, downSidePivotY) : new Vector2(pivotPos.x, upsidePivotY);

            rectTransform.pivot = pivotPos;

            Show();
        }

        private void SetTooltip(UIBaseSlot slot)
        {
            ItemData item = slot.UIItem.Item;

            // string buyPriceStr = string.Empty;

            // if (slot is UIShopSlot)
            // {
            //     buyPriceStr = string.Format("售价：＄{0}\n\n", item.BuyPrice);
            // }

            image.sprite = item.Icon;
            nameText.text = $"<color=white>{item.DisplayName}</color>";

            string contentStr = $"<color=green>{item.Description}</color>\n";

            tooltipText.text = contentStr;
            contentText.text = contentStr;
        }

        public void Show()
        {
            uiAnimation.PlayOpenAnimation();
            canvasGroup.alpha = 1.0f;
            isShow = true;
        }

        public void Hide()
        {
            uiAnimation.PlayCloseAnimation();
            canvasGroup.alpha = 0;
            isShow = false;
        }
    }
}