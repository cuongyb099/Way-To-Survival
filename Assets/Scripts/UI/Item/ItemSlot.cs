
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot :MonoBehaviour, IDropHandler
{
    public DragDropItem DropedItem { get; set; } = null;
    public Action<PointerEventData> OnDropItem;

    public void OnDrop(PointerEventData eventData)
    {
        OnDropItem?.Invoke(eventData);
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            DropedItem = eventData.pointerDrag.GetComponent<DragDropItem>();
        }
    }
}
