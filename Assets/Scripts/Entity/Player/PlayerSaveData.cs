
using System;
using System.Collections.Generic;
using KatInventory;
using Newtonsoft.Json;
using UnityEditor.Localization.Platform.Android;
using UnityEngine;

[Serializable]
public class PlayerSaveData
{
    [field:SerializeField] public string Username{get;set;}
    
    public int Coins
    {
        get => coins;
        set
        {
            coins = value;
            PlayerEvent.OnCoinChange?.Invoke(coins);
        }
    }
    [JsonIgnore]
    [SerializeField] private int coins;

    public int Diamonds
    {
        get => diamonds;
        set
        {
            diamonds = value;
            PlayerEvent.OnDiamondChange?.Invoke(diamonds);
        }
    }
    [JsonIgnore] 
    [SerializeField] private int diamonds;
    
    [field:SerializeField] public Inventory Inventory {get;private set;}
    [field:SerializeField] public List<string> IAPProductsID {get; private set;}
    public PlayerSaveData( string username, int money, int diamonds, InventorySO inventorySO)
    {
        Username = username;
        Coins = money;
        Diamonds = diamonds;
        Inventory = new Inventory(inventorySO);
    }
    [JsonConstructor]
    public PlayerSaveData( string username, int money, int diamonds, Inventory inventory, List<string> productsID )
    {
        Username = username;
        Coins = money;
        Diamonds = diamonds;
        Inventory = inventory;
        IAPProductsID = productsID;
    }
}
