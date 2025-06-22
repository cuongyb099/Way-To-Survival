
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDropItem:MonoBehaviour,IBeginDragHandler,IEndDragHandler,IPointerDownHandler,IDragHandler
{
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Transform parentAfterDrag;
    private int siblingIndexAfterDrag;
    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = false;
        parentAfterDrag = transform.parent;
        siblingIndexAfterDrag = transform.GetSiblingIndex();
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        ResetPosition();
    }

    public void OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void ResetPosition()
    {
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(parentAfterDrag);
        transform.SetSiblingIndex(siblingIndexAfterDrag);
    }
}
