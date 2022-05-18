using UnityEngine;
using UnityEngine.EventSystems;

namespace Ukiyo.UI.Interface
{
    public class OnHoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {

        public UIAnimation _background;

        // Background Hover Update
        public void OnPointerEnter(PointerEventData eventData)
        {
            _background.ListUIAnimationSet[0].finishValueOnOpen = transform.localPosition.y;
            _background.ListUIAnimationSet[0].finishValueOnClose = _background.transform.localPosition.y;
            _background.PlayOpenAnimation();;
        }

        // Background Hover Update
        public void OnPointerExit(PointerEventData eventData)
        {
            _background.ListUIAnimationSet[0].finishValueOnOpen = -820;
            _background.ListUIAnimationSet[0].finishValueOnClose = transform.localPosition.y;
            _background.PlayOpenAnimation();;
        }
    }
}