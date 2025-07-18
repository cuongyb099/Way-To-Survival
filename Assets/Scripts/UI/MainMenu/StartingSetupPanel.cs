using System.Collections.Generic;
using KatInventory;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class StartingSetupPanel : FadeBlurPanel
{
    [Header("Buttons")]
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Button BackButton;
    [Header("Content")]
    [SerializeField] private GameObject ListContent;
    [SerializeField] private WeaponItemUI ItemPrefab;
    [SerializeField] private LocalizedString FailToStartGameText;
    [SerializeField] private List<WeaponSlotUI> WeaponSlots;
    private List<WeaponItemUI> itemsUI = new List<WeaponItemUI>();

    protected override void OnAwake()
    {
        base.OnAwake();
        LoadButton();
    }

    public override void Show()
    {
        base.Show();
        
        LoadWeaponItems(PlayerDataPersistent.Instance.PlayerData.Inventory.GetItems(ItemType.Weapon));
    }

    public override void Hide()
    {
        base.Hide();

        foreach (var weapon in WeaponSlots)
        {
            weapon.ResetWeapon();
        }
    }

    private void LoadButton()
    {
        StartGameButton.onClick.AddListener(() =>
        {
            if (CheckStartGear())
            {
                PlayerDataPersistent.Instance.ChangeStartingWeapons(WeaponSlots[0].ItemBaseSoHolder,WeaponSlots[1].ItemBaseSoHolder,WeaponSlots[2].ItemBaseSoHolder);
                LoadingAsyncManager.Instance.SwitchToMap1();
            }
            else MessagePopup.Instance.ShowMessage(LocalizationSettings.StringDatabase.GetLocalizedString("UI Language Table", "You must select 3 weapons"));
        });
        BackButton.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.MainMenuPanel);
        });
    }
    
    private void LoadWeaponItems(IEnumerable<ItemData> items)
    {
        for (int i = itemsUI.Count-1; i >= 0; i--)
        {
            WeaponItemUI tmp = itemsUI[i];
            itemsUI.RemoveAt(i);
            Destroy(tmp.gameObject);
        }
        WeaponItemUI temp;
        foreach (var item in items)
        {
            temp = Instantiate(ItemPrefab, ListContent.transform);
            temp.Initialize((WeaponData)item);
            itemsUI.Add(temp);
        }
    }

    private bool CheckStartGear()
    {
        foreach (var x in WeaponSlots)
        {
            if(x.ItemBaseSoHolder == null)
                return false;
        }
        return true;
    }
}
