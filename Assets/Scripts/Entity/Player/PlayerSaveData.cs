
using System;
using System.Collections.Generic;
using KatInventory;

[Serializable]
public class PlayerSaveData
{
    public string UID;
    public string Username;
    public float Money;
    public Inventory Inventory;
    
    public PlayerSaveData(string uid, string username, float money, InventorySO inventorySO)
    {
        UID = uid;
        Username = username;
        Money = money;
        Inventory = new Inventory(inventorySO);
    }
}
