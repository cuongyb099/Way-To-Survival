
using System;
using System.Collections.Generic;
using KatInventory;

[Serializable]
public class PlayerSaveData
{
    public string UID;
    public string Username;
    public float Money;
    public List<ItemData> Inventory ;

    public PlayerSaveData()
    {
        Inventory = new List<ItemData>();
    }
    public PlayerSaveData(string uid, string username, float money, List<ItemData> inventory)
    {
        UID = uid;
        Username = username;
        Money = money;
        CreateInventory(inventory);
    }

    private void CreateInventory(List<ItemData> inventory)
    {
        Inventory = new List<ItemData>();
        foreach (var item in inventory)
        {
            Inventory.Add(item.StaticData.CreateItemData(item.Quantity));
        }
    }
}
