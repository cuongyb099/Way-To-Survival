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
        PlayerEvent.OnCashChange += UpdateTextMoney;
    }

    private void Start()
    {
        UpdateTextMoney(GameManager.Instance.Player.Money);
    }

    private void OnDestroy()
    {
        PlayerEvent.OnCashChange -= UpdateTextMoney;
    }

    private void UpdateTextMoney(float value)
    {
        textMoney.text = value.ToString();
    }
}
