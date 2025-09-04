using System.Collections.Generic;
using KatInventory;
using UnityEngine;

public class BuildingManagerUI : MonoBehaviour
{
    [SerializeField] protected BuildingItemUI itemUIPrefab;
    [SerializeField] protected Transform itemUIParent;

    protected List<BuildingItemUI> itemUis = new();
    protected List<TurretData> buildings;
    private void Awake()
    {
        buildings = PlayerDataPersistent.Instance.PlayerData.Inventory.GetItemsOfType<TurretData>();
        
        foreach (var building in buildings)
        {
            var clone = Instantiate(itemUIPrefab, itemUIParent);
            clone.SetData(building).SetNameLanguage(building.StaticData.Name).SetQuantity(building.Quantity);
            building.OnChangeQuantity += (quantity) =>
            {
                clone.SetQuantity(quantity);
            };
            clone.OnClick.AddListener((clickedItem) =>
            {
                if(clickedItem.Data.Quantity == 0) return;
                BuildingSystem.Instance.CurrentData = clickedItem.Data;
                BuildingSystem.Instance.SetMode(BuildingMode.BUILDING);
                this.gameObject.SetActive(false);
            });
            itemUis.Add(clone);
        }
    }
}