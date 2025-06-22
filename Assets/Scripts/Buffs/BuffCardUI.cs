using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BuffCardUI : MonoBehaviour
{
    public TextMeshProUGUI Name;
	public TextMeshProUGUI Description;
	public Image Icon;
	public Button Button;
	private BaseBuffSO buffData;
	public void Initialize(BaseBuffSO buff)
    {
        buffData = buff;
        Name.text = buff.Name.GetLocalizedString();
		Description.text = buff.Description.GetLocalizedString(buff.GetValues());
		Icon.sprite = buff.Icon;
	}
	public void BuffUpPlayer()
    {
	    buffData.AddStatusEffect(GameManager.Instance.Player.Stats);
    }
}
