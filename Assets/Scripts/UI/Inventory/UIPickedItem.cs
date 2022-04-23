using System.Collections.Generic;
using Ukiyo.UI.Slot;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Ukiyo.UI.Inventory
{
    public class UIPickedItem : MonoBehaviour
    {
        public UIBaseSlot pickUpSlot;
        
        public GraphicRaycaster graphicRaycaster;
        public InventorySystem inventorySystem;

        private Image image;
        private Text amountText;
        private EventSystem eventSystem;
        private CanvasGroup canvasGroup;
        private UIAnimation uiAnimation;

        private UIBaseSlot hitSlot = null;

        public bool isPickedItem { get; private set; }

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            canvasGroup.alpha = 0;

            image = GetComponent<Image>();
            amountText = GetComponentInChildren<Text>();

            eventSystem = EventSystem.current;
            uiAnimation = GetComponent<UIAnimation>();
        }

        private void Update()
        {
            if (isPickedItem)
            {
                transform.position = Input.mousePosition;

                PointerEventData eventData = new PointerEventData(eventSystem);
                eventData.pressPosition = Input.mousePosition;
                eventData.position = Input.mousePosition;

                List<RaycastResult> resultList = new List<RaycastResult>();
                graphicRaycaster.Raycast(eventData, resultList);


                bool isHitSlot = false;

                for (int i = 0; i < resultList.Count; i++)
                {
                    if (!isHitSlot)
                    {
                        hitSlot = resultList[i].gameObject.GetComponent<UIBaseSlot>();
                        if (hitSlot != null)
                            isHitSlot = true;
                    }
                }

                if (Input.GetMouseButtonUp(1))
                {
                    if (resultList.Count <= 0 && (pickUpSlot is InventorySlot)) //If no UI detected
                    {
                        DisposeItem();
                    }
                    //If the target is a inventory slot, the slot where the picked up item is located is also a inventory slot
                    else if (isHitSlot && (hitSlot is InventorySlot) && pickUpSlot is InventorySlot)
                    {
                        DropToOtherSlot(hitSlot); //Putting the item in another slot is essentially exchanging the data of the two slots
                    }
                    else //If none of the above conditions are met
                    {
                        CancelPickUp(); //Cancel the pickup, put the item back
                    }

                    OnDropItem(); //Reset a series of data within the class after completing the operation
                }
            }
        }

        public void PickUpItem(UIBaseSlot slot)
        {
            pickUpSlot = slot;

            image.sprite = slot.UIItem.Item.Icon;

            if (slot is InventorySlot)
            {
                if (slot.UIItem.Amount == 1)
                {
                    amountText.text = "";
                }
                else
                {
                    amountText.text = slot.UIItem.Amount.ToString();
                }
            }
            else
            {
                amountText.text = "";
            }


            canvasGroup.alpha = 1;
            canvasGroup.blocksRaycasts = true;
            uiAnimation.PlayOpenAnimation();
            isPickedItem = true;
            transform.position = Input.mousePosition;
            transform.SetAsLastSibling();

            slot.active = false;
        }

        private void CancelPickUp()
        {
            pickUpSlot.Show();
            isPickedItem = false;
        }

        private void DropToOtherSlot(UIBaseSlot targetSlot)
        {
            if (pickUpSlot != targetSlot)
            {
                inventorySystem.InventoryModule.SwapItem(pickUpSlot.SlotId, targetSlot.SlotId);
                inventorySystem.UpdatePanel();
            }

            pickUpSlot.Show();
            isPickedItem = false;
        }

        private void DisposeItem()
        {
            
        }
        
        private void OnDropItem()
        {
            image.sprite = null;
            amountText.text = "";
            canvasGroup.alpha = 0;
            canvasGroup.blocksRaycasts = false;
            isPickedItem = false;
        }

    }
}