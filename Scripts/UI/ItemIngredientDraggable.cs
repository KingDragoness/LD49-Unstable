using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BaksoGame
{

    public class ItemIngredientDraggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
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
        }

        public void OnDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition += eventData.delta / BaksoMainUI.instance.mainCanvas.scaleFactor;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            rectTransform.anchoredPosition = originPosImage;
            cg.blocksRaycasts = transform;

        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }


    }
}