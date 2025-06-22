using System;
using System.Collections;
using System.Collections.Generic;
using KatInventory;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

[RequireComponent(typeof(ItemSlot))]
public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI ItemName;
    [SerializeField] private Image ItemImage;
    [SerializeField] private Image BackgroundImage;
    [field: SerializeField]public Button ItemButton { get; private set; }
    public WeaponData ItemBaseSoHolder { get; private set; }
    private ItemSlot slot;
    private void Awake()
    {
        Initialize(null);
        slot = GetComponent<ItemSlot>();
        slot.OnDropItem += (x) =>
        {
            WeaponItemUI weaponItemUI = x.pointerDrag.GetComponent<WeaponItemUI>();
            if (weaponItemUI)
            {
                if (ItemBaseSoHolder != null)
                {
                    slot.DropedItem.gameObject.SetActive(true);
                    slot.DropedItem.ResetPosition();
                }
                Initialize(weaponItemUI.ItemBaseSoHolder);
                x.pointerDrag.SetActive(false);
            }
        };
    }

    public void Initialize(WeaponData weaponData)
    {
        if (weaponData == null)
        {
            ItemBaseSoHolder = null;
            ItemName.text = "Empty";
            ItemImage.sprite = null;
            ItemImage.color = new Color(0, 0, 0, 0);
            BackgroundImage.sprite = GameDataManager.Instance.ItemRarityBackground[Rarity.Common];
            return;
        }
        ItemBaseSoHolder = weaponData;
        if(ItemName)
            ItemName.text = weaponData.WeaponSO.Name.GetLocalizedString();
        ItemImage.sprite = weaponData.WeaponSO.Icon;
        ItemImage.color = new Color(1, 1, 1, 1);
        BackgroundImage.sprite = GameDataManager.Instance.ItemRarityBackground[weaponData.WeaponSO.Rarity];
    }

    public void ResetWeapon()
    {
        if(!slot || ItemBaseSoHolder == null) return;
        slot.DropedItem.gameObject.SetActive(true);
        slot.DropedItem.ResetPosition();
        slot.DropedItem = null;
        Initialize(null);
    }
}
