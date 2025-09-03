using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class InGameShopPanel : PanelToggleByCanvas
{
    [Header("UI Elements")] 
    [SerializeField] private Button _backBtn;
    [SerializeField] private TextMeshProUGUI _moneyText;
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
        PlayerEvent.OnCashChange += ChangeMoneyText;
    }
    
    public override void Show()
    {
        base.Show();
        _moneyText.text = $"{GameManager.Instance.Player.Money}$";
    }

    private void LoadButton()
    {
        _backBtn.onClick.AddListener(() =>
        {
            Hide();
            UIManager.Instance.ShowPanel(UIConstant.GameplayPanel);
        });
    }

    public void ChangeMoneyText(float amount)
    {
        _moneyText.text = $"{amount}$";
    }
}
