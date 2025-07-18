using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Tech.Logger;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GunShopUI : FadeBlurPanel
{

    [Header("UI Elements")]
    [SerializeField] private GameObject GunsPanel ;
    [SerializeField] private  WeaponShopDataUI GunDataUI;
    [SerializeField] private  Button BuyButton;
    [SerializeField] private  TextMeshProUGUI BuyButtonText;
    [SerializeField] private  TextMeshProUGUI CashText;
    [SerializeField] private  Button BackButton;
    [Header("Data")] 
    [SerializeField] private  WeaponListSO WeaponListSo;
    [SerializeField] private  AssetReferenceGameObject ItemMiniPrefab;

    [SerializeField] private WeaponSlotUI Selected;
    [SerializeField] private List<WeaponSlotUI> ItemsMiniUI;

    // protected override void OnAwake()
    // {
    //     base.OnAwake();
    //     Initialize();
    //     PlayerEvent.OnCashChange += ChangeCashText;
    //     ChangeCashText(GameManager.Instance.Player.Resin);
    // }
    //
    // private void OnDestroy()
    // {
    //     PlayerEvent.OnCashChange -= ChangeCashText;
    // }

    // private async void Initialize()
    // {
    //     ItemsMiniUI = new List<WeaponSlotUI>();
    //     
    //     BackButton.onClick.AddListener(() =>
    //     {
    //         Hide();
    //         UIManager.Instance.ShowPanel(UIConstant.MainGameplayPanel);
    //     });
    //     
    //     foreach (var x in WeaponListSo.Weapons)
    //     {
    //         var temp = await AddressablesManager.Instance.InstantiateAsyncType<WeaponSlotUI>(ItemMiniPrefab, GunsPanel.transform);
    //         temp.Initialize(x);
    //         temp.ItemButton.onClick.AddListener(() => { ChangeGun(temp);});
    //         ItemsMiniUI.Add(temp);
    //     }
    //     //ChangeGun(ItemsMiniUI[0]);
    //     
    // }
    
    // private void ChangeGun(WeaponSlotUI itemUI)
    // {
    //     Selected = itemUI;
    //     itemUI.ItemButton.Select();
    //     GunBaseSO weapon = (GunBaseSO)itemUI.ItemBaseSoHolder;
    //     //GunDataUI.ChangeGun(weapon);
    //     
    //     PlayerController player = GameManager.Instance.Player;
    //     // if (player.OwnedWeapons.Contains(weapon.Prefab.Data.GoReference))
    //     // {
    //     //     BuyButton.interactable = false;
    //     //     BuyButtonText.text = "Owned";
    //     // }
    //     // else
    //     // {
    //     //     BuyButton.interactable = true;
    //     //     BuyButtonText.text = "Buy";
    //     // }
    // }

    // public void OnBuyGun()
    // {
    //     PlayerController player = GameManager.Instance.Player;
    //     GunBaseSO weapon = (GunBaseSO)Selected.ItemBaseSoHolder;
    //     if (player.Cash < weapon.BuyPrice) return;
    //
    //     player.Cash -= weapon.BuyPrice;
    //     player.OwnedWeapons.Add(weapon.WeaponPrefab);
    //
    //     for(int i = 0; i< player.Weapons.Length; i++)
    //     {
    //         if (player.Weapons[i] != null) continue;
    //         player.InstantiateWeapon(weapon.WeaponPrefab,i);
    //         break;
    //     }
    //     
    //     BuyButton.interactable = false;
    //     BuyButtonText.text = "Owned";
    // }

    private void ChangeCashText(int value)
    {
        CashText.text = value+"$";
    }
}
