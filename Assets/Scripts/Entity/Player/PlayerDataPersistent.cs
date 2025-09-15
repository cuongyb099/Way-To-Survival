using System;
using System.Collections;
using System.IO;
using AYellowpaper.SerializedCollections;
using Firebase.Database;
using JsonSubTypes;
using KatInventory;
using Newtonsoft.Json;
using Tech.Json;
using Tech.Singleton;
using Unity.Entities;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class PlayerDataPersistent : SingletonPersistent<PlayerDataPersistent>
{
    public static readonly string path = "Assets/Save/PlayerData.json";
    private static readonly JsonSerializerSettings settings = new() {TypeNameHandling = TypeNameHandling.None };
    public string UserID { get; set; } = null;

    [field: SerializeField]
    public PlayerSaveData PlayerData { get; private set; }
    private DatabaseReference dbReference;
    [SerializeField] private InventorySO _beginningInventory;
    //
    [field:SerializeField] public int StartingResin { get;private set; }
    [field:SerializeField] public WeaponData[] StartingWeapons { get;private set; }
    [field:SerializeField] public BaseBuffSO[] StartingBuffs{ get;private set; }
    public Action OnSavePlayerData,OnLoadPlayerData;
    protected override void Awake()
    {
        base.Awake();
        StartingWeapons = new WeaponData[3]{null, null, null};
        StartingBuffs = new BaseBuffSO[4];
        
        dbReference = FirebaseDatabase.DefaultInstance.RootReference;
        //setup json
        
        settings.Converters.Add(JsonSubtypesConverterBuilder
            .Of<ItemData>("Type")
            .RegisterSubtype<ItemData>(SerializeItemType.DefaultItem)
            .RegisterSubtype<WeaponData>(SerializeItemType.Weapon)
            .RegisterSubtype<GunData>(SerializeItemType.Gun)
            .RegisterSubtype<TurretData>(SerializeItemType.Turret) 
            .SerializeDiscriminatorProperty()
            .Build());            
    }
    
    [ContextMenu("Save")]
    public void Save()
    {
        // if(UserID == null) return;
        // OnSavePlayerData?.Invoke();
        // string json = JsonConvert.SerializeObject(_playerData,Formatting.None,settings);
        // dbReference.Child("users").Child(UserID).SetRawJsonValueAsync(json); 
#if UNITY_EDITOR
        OnSavePlayerData?.Invoke();
        string json = JsonConvert.SerializeObject(PlayerData, Formatting.None, settings);
        Json.WriteAllText(path,json);
        AssetDatabase.Refresh();
#endif
    }
    [ContextMenu("Load")]
    public void Load()
    {
        // if(UserID == null) return;
        // StartCoroutine(LoadDataEnum());
#if UNITY_EDITOR
        if (!File.Exists(path))
            PlayerData = new PlayerSaveData("AuthHandle.Instance.User.DisplayName", 1000, 10, _beginningInventory);
        else
        {
            string json = File.ReadAllText(path);
            PlayerSaveData data = JsonConvert.DeserializeObject<PlayerSaveData>(json, settings);
            PlayerData = data;
            return;
        }
        
        AssetDatabase.Refresh();
        OnLoadPlayerData?.Invoke();
#endif
    }

    private IEnumerator LoadDataEnum()
    {
        var data = dbReference.Child("users").Child(UserID).GetValueAsync(); 
        yield return new WaitUntil(() => data.IsCompleted);
        DataSnapshot snapshot = data.Result;
        string jsonData = snapshot.GetRawJsonValue();
        if (jsonData != null)
        {
            PlayerData = JsonConvert.DeserializeObject<PlayerSaveData>(jsonData,settings);
        }
        else
        {
            PlayerData = new PlayerSaveData(AuthHandle.Instance.User.DisplayName, 1000, 10, _beginningInventory);
        }
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

        playerController.Money = StartingResin;
    }
    //Auto save
    // private void OnApplicationPause(bool pauseStatus)
    // {
    //     if(pauseStatus)
    //         Save();
    // }
    //
    // protected override void OnApplicationQuit()
    // {
    //     Save();
    //     base.OnApplicationQuit();
    // }
}