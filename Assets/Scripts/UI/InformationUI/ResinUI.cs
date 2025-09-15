using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class MoneyUI : MonoBehaviour
{
    [FormerlySerializedAs("textResin")] [SerializeField] private TextMeshProUGUI textMoney;

    private void Awake()
    {
        PlayerEvent.OnCoinChange += UpdateTextMoney;
    }

    private void Start()
    {
        UpdateTextMoney(GameManager.Instance.Player.Money);
    }

    private void OnDestroy()
    {
        PlayerEvent.OnCoinChange -= UpdateTextMoney;
    }

    private void UpdateTextMoney(int value)
    {
        textMoney.text = value.ToString();
    }
}
