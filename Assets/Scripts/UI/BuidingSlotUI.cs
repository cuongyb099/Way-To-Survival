using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuidingSlotUI : MonoBehaviour
{
    private Image icon;
    private int quantity;
    private TextMeshProUGUI text;
    public Action OnSlotBtnClick;
    
    private void LoadComponent()
    {
        icon = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnClick()
    {
        OnSlotBtnClick?.Invoke();
    }
    
    public void Init(Sprite sprite, int quantity, string name)
    {
        LoadComponent();
        this.icon.sprite = sprite;
        this.quantity = quantity;
        this.text.text = name;
    }
}
