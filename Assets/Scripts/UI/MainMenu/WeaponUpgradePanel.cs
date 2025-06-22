using System.Collections.Generic;
using KatInventory;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUpgradePanel : FadeBlurPanel
{
    [Header("Buttons")]
    [SerializeField] private Button UpgradeWeaponButton;
    [SerializeField] private Button BackButton;
    [SerializeField] private Button WeaponButton;
    [SerializeField] private Button MaterialsButton;
    [Header("Content")] 
    [SerializeField] private GunShopDataUI GunUI;
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
        if(GunUI.GunData == null) return;
        GunUI.GunData.UpgradeWeapon();
    }
    private void HandleMaterialsListClick(UIItem obj)
    {
        if(obj.ItemData == null) return;
        
        Inventory.Instance.AddItem(obj.ItemData.StaticData);
        obj.ResetData();
    }
    
    private void HandleUpgradeMaterialClick(UIItem obj)
    {
        PutInMaterialList(obj.ItemData);
    }
    private void HandleWeaponClick(UIItem obj)
    {
        var x = (WeaponData)obj.ItemData;
        if( x != null)
            GunUI.ChangeGun(x);
    }
    private void LoadItems(ItemType itemType)
    {
        IEnumerable<ItemData> items = Inventory.Instance.GetItems(itemType);
        if (!itemsUI.ContainsKey(itemType))
        {
            itemsUI.Add(itemType, new List<UIItem>());
        }
        
        for (int i = itemsUI[itemType].Count-1; i >= 0; i--)
        {
            UIItem tmp = itemsUI[itemType][i];
            itemsUI[itemType].RemoveAt(i);
            Destroy(tmp.gameObject);
        }
        UIItem temp;
        foreach (var item in items)
        {
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
    public void PutInMaterialList(ItemData itemData)
    {
        foreach (var slot in selectedMaterials)
        {
            if (slot.ItemData == null)
            {
                slot.ChangeItemDisplay(itemData,itemData.StaticData.Icon,1);
                Inventory.Instance.RemoveItem(itemData.StaticData);
                return;
            }
        }
    }
}
