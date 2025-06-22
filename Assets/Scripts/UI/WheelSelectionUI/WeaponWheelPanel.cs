using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponWheelPanel : FadeBlurPanel
{
    public List<ItemWheelUI> Items;
    [Header("Button")] 
    [SerializeField] private Button InventoryBtn;
    [SerializeField] private Button BackgroundBtn;
    
    protected virtual void Awake()
    {
        LoadButton();
                
        GameEvent.OnStartCombatState += DeactivateInventory;
        GameEvent.OnStopCombatState += ActivateInventory;
        
        InputEvent.OnInputWeaponWheel += ToggleWeaponWheelUI;
    }

    public void ToggleWeaponWheelUI()
    {
        UIManager.Instance.HidePanel(UIConstant.MainGameplayPanel);
        UIManager.Instance.ShowPanel(UIConstant.WeaponWheelPanel);
    }
    
    public override void Show()
    {
        base.Show();
        InitalizeItems();   
    }

    public void InitalizeItems()
    {
        WeaponBase[] guns = GameManager.Instance.Player.Weapons;
        //0->2 is for guns
        for (int i = 0; i < 3; i++)
        {
            Items[i].Initialize(i, guns[i] != null ? guns[i].WeaponData.WeaponSO.Name.GetLocalizedString() : null,
                guns[i] != null ? guns[i].WeaponData.WeaponSO.Icon : null);
        }
        
    }
    private void LoadButton()
    {
        InventoryBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.InventoryPanel);
        });
        BackgroundBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.MainGameplayPanel);
        });
        Items[0].ItemButton.onClick.AddListener(()=>
        {
            GameManager.Instance.Player.SwitchWeapon(0);
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.MainGameplayPanel);
        });
        Items[1].ItemButton.onClick.AddListener(()=>
        {
            GameManager.Instance.Player.SwitchWeapon(1);
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.MainGameplayPanel);
        });
        Items[2].ItemButton.onClick.AddListener(()=>
        {
            GameManager.Instance.Player.SwitchWeapon(2);
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.MainGameplayPanel);
        });
    }
    private void OnDestroy()
    {
        GameEvent.OnStartCombatState -= DeactivateInventory;
        GameEvent.OnStopCombatState -= ActivateInventory;
        
        InputEvent.OnInputWeaponWheel -= ToggleWeaponWheelUI;
    }
    
    private void ActivateInventory()
    {
        InventoryBtn.interactable = true;
    }
    private void DeactivateInventory()
    {
        InventoryBtn.interactable = false;
    }
}
