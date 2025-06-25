using System.Collections.Generic;
using KatInventory;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Inventory/new Inventory SO")]
public class InventorySO : ScriptableObject
{
    [field: SerializeField] public List<StartItem> ItemDataList { get; private set; }
}

[System.Serializable]
public class StartItem
{
    public ItemBaseSO Item;
    public int Quantity;
}
