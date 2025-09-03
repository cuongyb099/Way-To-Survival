
using System;
using KatInventory;
using UnityEngine;

[CreateAssetMenu(fileName = "EnhancerShopItemSO",menuName ="Item/Shop/new EnhancerShopItemSO")]
public class EnhancerShopItemSO : ShopBuyItemSO
{
    [field: Header("Enhancer Data")]
    [field:SerializeField] public Rarity Rarity { get; private set; }

    public override void OnBuyItem()
    {
        if (Rarity == Rarity.All)
        {
            BuffPanel.Instance.InitializeAllRandom();
        }
        else BuffPanel.Instance.InitializeAllWithRarity(Rarity);
    }
}
