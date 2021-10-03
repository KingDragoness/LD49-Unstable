using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BaksoGame
{

    public class PanciCookerDrop : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public void OnDrop(PointerEventData eventData)
        {
            if (eventData.pointerDrag == null)
            {
                return;
            }

            var someRectTransform = eventData.pointerDrag.GetComponent<RectTransform>();
            var draggable1 = someRectTransform.GetComponent<ItemIngredientDraggable>();

            if (draggable1 == null)
            {
                return;
            }

            var IngredientPutButton = draggable1.transform.parent.GetComponent<IngredientPutButton>();

            if (IngredientPutButton == null)
            {
                return;
            }

            OrderanUI.instance.Pot_NewIngredient(IngredientPutButton.itemID);
            OrderanUI.instance.RefreshTooltip();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            //tooltip
            OrderanUI.instance.tooltipText.gameObject.SetActive(true);
            OrderanUI.instance.RefreshTooltip();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OrderanUI.instance.tooltipText.gameObject.SetActive(false);

        }
    }
}