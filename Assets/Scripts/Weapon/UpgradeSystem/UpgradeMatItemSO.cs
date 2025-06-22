using System.Collections;
using System.Collections.Generic;
using KatInventory;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeMatItemSO", menuName = "Item/Materials/UpgradeMatItemSO")]
public class UpgradeMatItemSO : ItemBaseSO
{
    [field:Header("Upgrade Mat Data")]
    [field: SerializeField] public float UpgradeValue { get; private set; }

    public override ItemType GetItemType()
    {
        return ItemType.UpgradeMaterials;
    }
}
