using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.Purchasing;
using UnityEngine.UI;

public class MainMenuShopPanel : PanelToggleByCanvas
{
    [Header("UI Elements")] 
    [SerializeField] private Button _backBtn;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _diamondText;
    [Header("Common Panel Items")] 
    [SerializeField] private Button _smallHealButton;
    [SerializeField] private Button _mediumHealButton;
    [SerializeField] private Button _bigHealButton;
    [SerializeField] private Button _smallAmmoButton;
    [SerializeField] private Button _mediumAmmoButton;
    [SerializeField] private Button _bigAmmoButton;
    [Header("Enhancement Panel Items")] 
    [SerializeField] private Button _randomEnhancerButton;
    [SerializeField] private Button _basicEnhancerButton;
    [SerializeField] private Button _advanceEnhancerButton;
    [SerializeField] private Button _highEnhancerButton;
    [SerializeField] private Button _topSecretEnhancerButton;
    
    protected override void OnAwake()
    {
        base.OnAwake();
        LoadButton();
        PlayerEvent.OnCoinChange += ChangeCoinText;
        PlayerEvent.OnDiamondChange += ChangeDiamondText;
    }
    
    public override void Show()
    {
        base.Show();
        _coinText.text = $"{PlayerDataPersistent.Instance.PlayerData.Coins}";
        _diamondText.text = $"{PlayerDataPersistent.Instance.PlayerData.Diamonds}";
    }

    private void LoadButton()
    {
        _backBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.MainMenuPanel);
            
        });
    }

    public void ChangeCoinText(int amount)
    {
        _coinText.text = $"{amount}";
    }
    public void ChangeDiamondText(int amount)
    {
        _diamondText.text = $"{amount}";
    }
    
}