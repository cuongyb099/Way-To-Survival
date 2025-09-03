
using System;
using System.Collections.Generic;
using KatInventory;
using Newtonsoft.Json;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    public string Username;
    public float Money;
    public int Diamonds;
    public Inventory Inventory;
    
    public PlayerSaveData( string username, float money, int diamonds, InventorySO inventorySO)
    {
        Username = username;
        Money = money;
        Diamonds = diamonds;
        Inventory = new Inventory(inventorySO);
    }
    [JsonConstructor]
    public PlayerSaveData( string username, float money, int diamonds, Inventory inventory)
    {
        Username = username;
        Money = money;
        Diamonds = diamonds;
        Inventory = inventory;
    }
}
