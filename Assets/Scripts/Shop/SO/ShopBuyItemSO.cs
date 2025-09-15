using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;

public abstract class ShopBuyItemSO : ScriptableObject
{
    [field: SerializeField] public LocalizedString ItemName { get; private set; }
    [field: SerializeField] public LocalizedString ItemDescription { get; private set; }
    [field: SerializeField] public Sprite ItemIcon { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    public abstract void OnBuyItem();
}
