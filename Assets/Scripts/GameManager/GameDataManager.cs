using AYellowpaper.SerializedCollections;
using KatInventory;
using Tech.Singleton;
using UnityEngine;

public class GameDataManager : SingletonPersistent<GameDataManager>
{
    [field: SerializeField] public AnimationCurve UpgradeCurve { get; private set; }
    [field: SerializeField] public BuffListSO BuffListSO { get; private set; }
    [SerializeField] public SerializedDictionary<Rarity, Sprite> ItemRarityBackground = new();
}
