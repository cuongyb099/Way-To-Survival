using System.Collections;
using System.Collections.Generic;
using KatInventory;
using UnityEngine;

[CreateAssetMenu(fileName = "InventorySO", menuName = "Inventory/new Inventory SO")]
public class InventorySO : ScriptableObject
{
    [field: SerializeField] public List<ItemData> ItemDataList { get; private set; }
}
