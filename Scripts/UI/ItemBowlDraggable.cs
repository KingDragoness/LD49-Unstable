using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BaksoGame
{
    public class ItemBowlDraggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IPointerUpHandler
    {
        public BowlButton parentButton;
        public CanvasGroup cg;

        private Vector2 originPosImage;
        private RectTransform rectTransform;

        private void Start()
        {
            rectTransform = GetComponent<RectTransform>();
            originPosImage = rectTransform.anchoredPosition;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            cg.blocksRaycasts = false;
            OrderanUI.instance.currentHoldBowl = parentButton;
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / BaksoMainUI.instance.mainCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition = originPosImage;
            cg.blocksRaycasts = transform;
            OrderanUI.instance.currentHoldBowl = null;
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Debug.Log("Drop item");
            OrderanUI.instance.GiveBowlToCustomer(parentButton);
        }


    }
}