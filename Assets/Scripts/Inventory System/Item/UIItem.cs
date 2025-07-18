using System;
using System.Collections;
using System.Collections.Generic;
using KatInventory;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIItem : MonoBehaviour,IBeginDragHandler, IDragHandler,IEndDragHandler,IPointerClickHandler,IDropHandler
{
    [field: SerializeField] public Image ItemImage { get; private set; }
    [field: SerializeField] public Image BorderImage { get; private set; }
    [field: SerializeField] public Image BackgroundImage { get; private set; }
    [field: SerializeField] public TextMeshProUGUI ItemCount { get; private set; }
    public ItemData ItemData { get; private set; }
    public Action<UIItem> OnItemClick,OnItemBeginDrag,OnItemDrop,OnItemEndDrag;
    
    public void Initialize()
    {
        ResetData();
        Deselect();
    }
    
    public void ResetData()
    {
        ItemData = null;
        ItemCount.text = "";
        ItemImage.gameObject.SetActive(false);
        BackgroundImage.sprite = GameDataManager.Instance.ItemRarityBackground[Rarity.Common];
    }
    public void ChangeItem(ItemData item)
    {
        if(item == null) return;
        ItemData = item;
        ItemImage.sprite = item.StaticData.Icon;
        if (item.Quantity <= 1)
            ItemCount.text = "";
        else
            ItemCount.text = item.Quantity.ToString();
        BackgroundImage.sprite = GameDataManager.Instance.ItemRarityBackground[item.StaticData.Rarity];
        ItemImage.gameObject.SetActive(true);
    }
    public void ChangeItemDisplay(ItemData data, Sprite image, int quantity)
    {
        ItemData = data;
        ItemImage.sprite = image;
        UpdateItemQuantity(quantity, data.StaticData.Unique);
        BackgroundImage.sprite = GameDataManager.Instance.ItemRarityBackground[data.StaticData.Rarity];
        ItemImage.gameObject.SetActive(true);
    }

    private void UpdateItemQuantity(int quantity, bool unique)
    {
        if(unique)
            ItemCount.text = "";
        else
            ItemCount.text = quantity.ToString();
    }
    public void ForceUpdateItemQuantity(int ammount)
    { 
        var x = int.Parse(ItemCount.text) + ammount;
        ItemCount.text = x.ToString();
    }
    public void Select()
    {
        BorderImage.enabled = true;
    }
    public void Deselect()
    {
        BorderImage.enabled = false;
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        Select();
        OnItemBeginDrag?.Invoke(this);
    }
    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Deselect();
        OnItemEndDrag?.Invoke(this);
    }
    
    public void OnDrop(PointerEventData eventData)
    {
        OnItemDrop?.Invoke(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        OnItemClick?.Invoke(this);

    }


}
