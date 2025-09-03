
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ShopItemUI: MonoBehaviour
{
    [field: SerializeField] public Image Image { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Name { get; private set; }
    [field: SerializeField] public TextMeshProUGUI Price { get; private set; }
    private Button button;
    [SerializeField] private ShopBuyItemSO _shopBuyItemSO;
    [SerializeField] private AudioClip _buyAudio;
    
    private LocalizedString localizedString;
    private StringVariable name;
    private FloatVariable price;
    private void Awake()
    {
        button = GetComponent<Button>();
        Name.text = _shopBuyItemSO.ItemName.GetLocalizedString();
        Price.text = ((int)_shopBuyItemSO.Price) +"$";
        Image.sprite = _shopBuyItemSO.ItemIcon;
        button.onClick.AddListener(Buy);
        SetUpHeader();
    }

    private void SetUpHeader()
    {
        name = new StringVariable();
        price = new FloatVariable();
        localizedString = new LocalizedString();
        localizedString.SetReference("UI Language Table", "On Buy Item Confirm Header");
        localizedString.Add("name", name);
        localizedString.Add("price", price);
    }
    
    private void Buy()
    {
        name.Value = _shopBuyItemSO.ItemName.GetLocalizedString();
        price.Value = _shopBuyItemSO.Price;
        
        ConfirmPopUp.Instance.Show(ConfirmPopUpType.YesNo, 
            localizedString.GetLocalizedString(),_shopBuyItemSO.ItemDescription.GetLocalizedString(),
            () =>
            {
                if (_shopBuyItemSO.Price > GameManager.Instance.Player.Money)
                {
                    MessagePopup.Instance.ShowMessage(
                        LocalizationSettings.StringDatabase.GetLocalizedString("UI Language Table", "Sorry, M.E.D Corp doesn't serve the poor"));
                }
                else
                {
                    GameManager.Instance.Player.Money -= _shopBuyItemSO.Price;
                    _shopBuyItemSO.OnBuyItem();
                    AudioManager.Instance.PlaySound(_buyAudio,volumeType: SoundVolumeType.UI_VOLUME);
                }
            });
    }
}
