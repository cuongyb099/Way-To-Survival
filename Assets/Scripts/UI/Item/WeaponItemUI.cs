using System.Collections;
using System.Collections.Generic;
using KatInventory;
using UnityEngine;
using UnityEngine.UI;

public class WeaponItemUI : MonoBehaviour
{
    [SerializeField] private Image ItemImage;
    [SerializeField] private Image BackgroundImage;
    public WeaponData ItemBaseSoHolder { get; private set; }
    
    public void Initialize(WeaponData weaponData)
    {
        if (weaponData == null)
        {
            ItemBaseSoHolder = null;
            ItemImage.sprite = null;
            ItemImage.color = new Color(0, 0, 0, 0);
            BackgroundImage.sprite = GameDataManager.Instance.ItemRarityBackground[Rarity.Common];
            return;
        }
        ItemBaseSoHolder = weaponData;
        ItemImage.sprite = weaponData.WeaponSO.Icon;
        ItemImage.color = new Color(1, 1, 1, 1);
        BackgroundImage.sprite = GameDataManager.Instance.ItemRarityBackground[weaponData.WeaponSO.Rarity];
    }
}
