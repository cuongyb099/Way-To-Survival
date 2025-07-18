using System;
using System.IO;
using KatInventory;
using Tech.Json;
using Tech.Singleton;
using UnityEngine;

public class PlayerDataPersistent : SingletonPersistent<PlayerDataPersistent>,ISaveable
{
    public static readonly string DefaultPath = System.IO.Path.Combine(Application.streamingAssetsPath, "SaveFile/BaseData.json");
    public static readonly string SavePath = "Assets/Save/PlayerData.json";
    public PlayerSaveData PlayerData
    {
        get => _playerData;
        set => _playerData = value;
    }
    [SerializeField] private PlayerSaveData _playerData;
    [SerializeField] private InventorySO _beginningInventory;
    [field:SerializeField] public int StartingResin { get;private set; }
    //
    [field:SerializeField] public WeaponData[] StartingWeapons { get;private set; }
    [field:SerializeField] public BaseBuffSO[] StartingBuffs{ get;private set; }
    public Action OnSavePlayerData,OnLoadPlayerData;
    protected override void Awake()
    {
        base.Awake();
        StartingWeapons = new WeaponData[3]{null, null, null};
        StartingBuffs = new BaseBuffSO[4];
        ItemDataBase.OnLoadDone += Load;
    }

    private void OnDestroy()
    {
        ItemDataBase.OnLoadDone -= Load;
    }

    [ContextMenu("Save")]
    public void Save()
    {
        OnSavePlayerData?.Invoke();
        PlayerData.SaveJson(SavePath);
    }
    [ContextMenu("Load")]
    public void Load()
    {
        if (!File.Exists(SavePath))
        {
            _playerData = new PlayerSaveData("0","defaultUser",0f,_beginningInventory);
        }
        else
            Json.LoadJson(SavePath, out _playerData);
        OnLoadPlayerData?.Invoke();
    }
    
    public void ChangeStartingWeapons(WeaponData[] weaponDatas)
    {
        StartingWeapons = weaponDatas;
    }
    public void ChangeStartingWeapons(WeaponData weapon1,WeaponData weapon2,WeaponData weapon3)
    {
        StartingWeapons[0] = weapon1;
        StartingWeapons[1] = weapon2;
        StartingWeapons[2] = weapon3;
    }
    public void ChangeStartingBuffs(BaseBuffSO[] buffBaseSo)
    {
        StartingBuffs = buffBaseSo;
    }

    public void ApplyToPlayer(PlayerController playerController)
    {
        int i;
        for (i = 0; i < StartingWeapons.Length; i++)
        {
            if (StartingWeapons[i] != null){
                playerController.InstantiateWeapon(StartingWeapons[i],i);
            }
        }
        for (i = 0; i < StartingBuffs.Length; i++)
        {
            if (StartingBuffs[i] != null)
                StartingBuffs[i].AddStatusEffect(playerController.Stats);
        }

        playerController.Resin = StartingResin;
    }
}