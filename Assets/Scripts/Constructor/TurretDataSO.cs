using KatInventory;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/Building/Turret")]
public class TurretDataSO : ItemGOBaseSO
{
    public override ItemType GetItemType() => ItemType.Building;

    public override ItemData CreateItemData(int quantity = 1)
    {
        return new TurretData(this, quantity);   
    }
}
