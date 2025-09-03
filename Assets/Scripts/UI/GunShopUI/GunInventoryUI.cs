using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class GunInventoryUI : FadeBlurPanel
{

    [Header("UI Elements")] 
    public GameObject GunsPanel;
    public WeaponShopDataUI GunDataUI;
    public Button BackButton;
    [Header("Data")]
    public AssetReferenceGameObject GunMiniPrefab;

    public List<WeaponSlotUI> GunEquipedSlots;

    public WeaponSlotUI Selected { get; private set; }
    public List<WeaponSlotUI> GunsMiniUI { get; private set; }
    private PlayerController player;

    // protected override void OnAwake()
    // {
    //     base.OnAwake();
    //     GunsMiniUI = new List<WeaponSlotUI>();
    //     player = GameManager.Instance.Player;
    //     LoadButton();
    // }
    // private void LoadButton()
    // {
    //     BackButton.onClick.AddListener(() =>
    //     {
    //         Hide();
    //         UIManager.Instance.ShowPanel(UIConstant.MainGameplayPanel);
    //     });
    //     
    //     for (int i = 0; i < GunEquipedSlots.Count; i++)
    //     {
    //         var x = i;
    //         GunEquipedSlots[i].ItemButton.onClick.AddListener(()=>InitEquippedSlots(x));
    //     }
    // }
    //
    // public override void Show()
    // {
    //     base.Show();
    //     Initialize();
    // }
    //
    // private async void Initialize()
    // {
    //     //delete gun panel
    //     for(int i = GunsMiniUI.Count - 1; i >= 0; i--)
    //     {
    //         WeaponSlotUI g = GunsMiniUI[i];
    //         g.ItemButton.onClick.RemoveAllListeners();
    //         GunsMiniUI.RemoveAt(i);
    //         Destroy(g.gameObject);
    //     }
    //     //instantiate gun panel
    //     // foreach (var x in player.OwnedWeapons)
    //     // {
    //     //     WeaponSlotUI temp = await AddressablesManager.Instance.InstantiateAsyncType<WeaponSlotUI>(GunMiniPrefab, GunsPanel.transform);
    //     //     temp.Initialize(x.WeaponData);
    //     //     temp.ItemButton.onClick.AddListener(new UnityEngine.Events.UnityAction(() => { ChangeGun(temp);}));
    //     //     GunsMiniUI.Add(temp);
    //     // }
    //
    //     for (int i = 0; i < player.Weapons.Length; i++)
    //     {
    //         GunEquipedSlots[i].Initialize(player.Weapons[i] != null?player.Weapons[i].WeaponData.WeaponSO:null);
    //     }
    //     ChangeGun(GunsMiniUI[0]);
    // }
    //
    // private void ChangeGun(WeaponSlotUI gunUI)
    // {
    //     Selected = gunUI;
    //     GunBaseSO weapon = (GunBaseSO)gunUI.ItemBaseSoHolder;
    //     gunUI.ItemButton.Select();
    //     //GunDataUI.ChangeGun(weapon);
    // }
    // private void InitEquippedSlots(int index)
    // {
    //     WeaponBase weapon = (WeaponBase)Selected.ItemBaseSoHolder.Prefab ;
    //     if (!player.ContainsWeapon(weapon))
    //     {
    //         player.InstantiateWeapon(Selected.ItemBaseSoHolder,index);
    //         GunEquipedSlots[index].Initialize(Selected.ItemBaseSoHolder);
    //     }
    //
    // }
}
