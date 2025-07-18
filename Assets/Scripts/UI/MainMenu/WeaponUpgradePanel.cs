using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.Math;
using KatInventory;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;
using Random = System.Random;

public class WeaponUpgradePanel : FadeBlurPanel
{
    [Header("Buttons")]
    [SerializeField] private Button UpgradeWeaponButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button WeaponButton;
    [SerializeField] private Button MaterialsButton;
    [Header("Content")] 
    [SerializeField] private WeaponShopDataUI GunUI;
    [SerializeField] private GameObject MaterialPanel;
    [SerializeField] private GameObject ListContent;
    [SerializeField] private UIItem ItemPrefab;
    
    private Dictionary<ItemType,List<UIItem>> itemsUI;
    public List<UIItem> selectedMaterials;

    protected override void OnAwake()
    {
        base.OnAwake();
        MaterialPanel.GetComponentsInChildren(selectedMaterials);
        itemsUI = new Dictionary<ItemType, List<UIItem>>();
        LoadButton();
    }

    public override void Show()
    {
        base.Show();        
        LoadItems(ItemType.Weapon);
        LoadItems(ItemType.UpgradeMaterials);
        
        ShowItemTypeOnly(ItemType.Weapon);
    }

    private void LoadButton()
    {
        UpgradeWeaponButton.onClick.AddListener(() =>
        {
            HandleUpgradeButtonClick();
        });
        BackButton.onClick.AddListener(() =>
        {
            ClearMaterialList();
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.MainMenuPanel);
        });
        WeaponButton.onClick.AddListener(() =>
        {
            ShowItemTypeOnly(ItemType.Weapon);
        });
        MaterialsButton.onClick.AddListener(() =>
        {
            ShowItemTypeOnly(ItemType.UpgradeMaterials);
        });
        
        foreach (var slot in selectedMaterials)
        {
            slot.OnItemClick += HandleMaterialsListClick;
            slot.Initialize();
        }
    }
    private void HandleUpgradeButtonClick()
    {
        if (GunUI.WeaponData == null)
        {
            MessagePopup.Instance.ShowMessage(
                LocalizationSettings.StringDatabase.GetLocalizedString("UI Language Table", "Select a weapon"));
            return;
        }
        if(!CheckUpgradable())
            return;
        GunUI.WeaponData.UpgradeWeapon();
        GunUI.UpdateGunStats();
        MessagePopup.Instance.ShowMessage("Upgraded " + GunUI.WeaponData.StaticData.Name.GetLocalizedString());
    }

    private bool CheckUpgradable()
    {
        if (GunUI.WeaponData.WeaponLevel >= 15)
        {
            MessagePopup.Instance.ShowMessage(
                LocalizationSettings.StringDatabase.GetLocalizedString("UI Language Table", "Max Weapon Level"));
            return false;
        }

        if (!UpgradeGacha())
        {
            MessagePopup.Instance.ShowMessage(
                LocalizationSettings.StringDatabase.GetLocalizedString("UI Language Table", "Upgrade Weapon Failed"));
            return false;
        }
        
        return true;
    }

    private bool UpgradeGacha()
    {
        int gachaNum = 0;
        foreach (var slot in selectedMaterials)
        {
            if (slot.ItemData != null)
            {
                switch (slot.ItemData.StaticData.Rarity)
                {
                    case Rarity.Common:
                        gachaNum+=1;
                        break;
                    case Rarity.Uncommon:
                        gachaNum+=2;
                        break;
                    case Rarity.Rare:
                        gachaNum+=3;
                        break;
                    case Rarity.ExtremelyRare:
                        gachaNum+=5;
                        break;
                    case Rarity.Myth:
                        gachaNum+=10;
                        break;
                }
                foreach (var item in itemsUI[ItemType.UpgradeMaterials])
                {
                    if (item.ItemData == slot.ItemData)
                    {
                        item.ItemData.Quantity -= 1;
                        slot.ResetData();
                        break;
                    }
                }
            }
        }
        
        if(UnityEngine.Random.value < gachaNum/(4f*(GunUI.WeaponData.WeaponLevel+1)))
            return true;
        return false;
    }
    private void HandleMaterialsListClick(UIItem obj)
    {
        if(obj.ItemData == null) return;
        foreach (var item in itemsUI[ItemType.UpgradeMaterials])
        {
            if (item.ItemData == obj.ItemData)
            {
                item.ForceUpdateItemQuantity(1);
                obj.ResetData();
                break;
            }
        }
    }
    
    private void HandleUpgradeMaterialClick(UIItem obj)
    {
        PutInMaterialList(obj);
    }
    private void HandleWeaponClick(UIItem obj)
    {
        var x = (WeaponData)obj.ItemData;
        if( x != null)
            GunUI.ChangeGun(x);
    }
    private void LoadItems(ItemType itemType)
    {
        IEnumerable<ItemData> items = PlayerDataPersistent.Instance.PlayerData.Inventory.GetItems(itemType);
        //dictionary init
        if (!itemsUI.ContainsKey(itemType))
        {
            itemsUI.Add(itemType, new List<UIItem>());
        }
        //destroy items
        for (int i = itemsUI[itemType].Count-1; i >= 0; i--)
        {
            UIItem tmp = itemsUI[itemType][i];
            itemsUI[itemType].RemoveAt(i);
            Destroy(tmp.gameObject);
        }
        //add items
        UIItem temp;
        foreach (var item in items)
        {
            //dont spawn special weapons
            if (itemType == ItemType.Weapon)
            {
                var tmp = (WeaponBaseSO)item.StaticData;
                if (!tmp || tmp.WeaponType is WeaponType.SpecialWeapon)
                    continue;
            }
            
            temp = Instantiate(ItemPrefab, ListContent.transform);
            temp.ChangeItem(item);
            switch (itemType)
            {
                case ItemType.UpgradeMaterials:
                    temp.OnItemClick += HandleUpgradeMaterialClick;
                    break;
                case ItemType.Weapon:
                    temp.OnItemClick += HandleWeaponClick;
                    break;
            }
            itemsUI[itemType].Add(temp);
        }
    }

    public void ShowItemTypeOnly(ItemType itemType)
    {
        foreach (var key in itemsUI.Keys)
        {
            if (key != itemType)
            {
                foreach (var uiItem in itemsUI[key])
                {
                    uiItem.gameObject.SetActive(false);
                }
            }
            else
            {
                foreach (var uiItem in itemsUI[key])
                {
                    uiItem.gameObject.SetActive(true);
                }
            }
        }
    }
    public void ClearMaterialList()
    {
        foreach (var slot in selectedMaterials)
        {
            if (slot.ItemData != null)
            {
                foreach (var item in itemsUI[ItemType.UpgradeMaterials])
                {
                    if (item.ItemData == slot.ItemData)
                    {
                        item.ForceUpdateItemQuantity(1);
                        slot.ResetData();
                        break;
                    }
                }
            }
        }
    }
    public void PutInMaterialList(UIItem uiItem)
    {
        foreach (var slot in selectedMaterials)
        {
            if (slot.ItemData == null)
            {
                uiItem.ForceUpdateItemQuantity(-1);
                slot.ChangeItemDisplay(uiItem.ItemData,uiItem.ItemData.StaticData.Icon,1);
                return;
            }
        }
    }
}
