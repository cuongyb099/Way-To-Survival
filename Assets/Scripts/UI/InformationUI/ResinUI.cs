using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResinUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textResin;

    private void Awake()
    {
        PlayerEvent.OnCashChange += UpdateTextResin;
    }

    private void Start()
    {
        UpdateTextResin(GameManager.Instance.Player.Resin);
    }

    private void OnDestroy()
    {
        PlayerEvent.OnCashChange -= UpdateTextResin;
    }

    private void UpdateTextResin(int value)
    {
        textResin.text = value.ToString();
    }
}
