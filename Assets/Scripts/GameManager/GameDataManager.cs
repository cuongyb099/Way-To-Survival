using System.Collections;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using KatInventory;
using Tech.Singleton;
using UnityEngine;

public class GameDataManager : SingletonPersistent<GameDataManager>
{
    [field: SerializeField] public BuffListSO BuffListSO { get; private set; }
    [SerializeField] public SerializedDictionary<Rarity, Sprite> ItemRarityBackground = new();
}
