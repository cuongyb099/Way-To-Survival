using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatInfoButton : MonoBehaviour
{
    [SerializeField] private Image StatImage;
    [SerializeField] private TextMeshProUGUI StatName;
    [SerializeField] private TextMeshProUGUI StatNum;
    [SerializeField] private StatType type;
    [SerializeField] private bool percentage;
    private Stat stat;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        stat = GameManager.Instance.Player.Stats.GetStat(type);
        stat.OnValueChange += UpdateValues;
        button.onClick.AddListener(ShowMoreInfo);
        UpdateValues();
    }
    private void UpdateValues()
    {
        if (type is StatType.MaxBulletPoints)
        {
            StatNum.text = ((int)stat.Value).ToString();
            return;
        }
        if(percentage)
            StatNum.text = (stat.Value).ToString("P1");
        else
            StatNum.text = stat.Value.ToString("F1");
    }
    public void ShowMoreInfo()
    {
        ConfirmPopUp.Instance.Show(ConfirmPopUpType.Confirm,StatName.text,
            $"(<size=60%><voffset=20><color=#F4B900>Base Value</color></voffset></size>{stat.BaseValue} + <size=60%><voffset=20><color=#F4B900>Base Flat</color></voffset></size>{stat._baseFlatSum}) x <size=60%><voffset=20><color=#F4B900>Percentage</color></voffset></size>{100+stat._percentageSum}% + <size=60%><voffset=20><color=#F4B900>Flat</color></voffset></size>{stat._flatSum} = {((percentage)?stat.Value.ToString("P1"):stat.Value.ToString("F1"))}");
    }
}
