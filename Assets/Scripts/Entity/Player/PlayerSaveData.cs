
using System;
using System.Collections.Generic;
using KatInventory;

[Serializable]
public class PlayerSaveData
{
    public string UID;
    public string Username;
    public float Money;
    public List<StartItem> Inventory ;

    public PlayerSaveData()
    {
        Inventory = new List<StartItem>();
    }
    public PlayerSaveData(string uid, string username, float money, List<StartItem> inventory)
    {
        UID = uid;
        Username = username;
        Money = money;
        Inventory = inventory;
    }
}
