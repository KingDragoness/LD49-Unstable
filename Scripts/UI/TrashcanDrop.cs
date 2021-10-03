using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BaksoGame
{
    public class TrashcanDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {

        public Animator trashcanAnimator;

        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            var someRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            var draggable1 = someRectTransform.GetComponent<ItemBowlDraggable>();

            if (draggable1 == null)
            {
                return;
            }

            var IngredientPutButton = draggable1.transform.parent.GetComponent<BowlButton>();

            if (IngredientPutButton == null)
            {
                return;
            }

            OrderanUI.instance.ThrowBowl(draggable1.parentButton);
            OrderanUI.instance.RefreshTooltip();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            trashcanAnimator.SetBool("highlight", true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            trashcanAnimator.SetBool("highlight", false);

        }
    }
}