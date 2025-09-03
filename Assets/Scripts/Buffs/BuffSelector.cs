using BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using KatInventory;
using UnityEngine;
using UnityEngine.Rendering;


public class BuffSelector : MonoBehaviour
{
	[SerializeField] private BuffListSO BuffsData;
	[SerializeField] private int CardCount = 3;
	private List<BuffCardUI> Cards;
	private void Awake()
	{
		Cards = new List<BuffCardUI>();
	}
	public void DestroyAll()
	{
		for(int i = Cards.Count - 1; i >= 0; i--)
		{
			BuffCardUI card = Cards[i];
			Cards.RemoveAt(i);
			card.Button.onClick.RemoveListener(DeactivateParent);
			Destroy(card.gameObject);
		}
	}
	public void InitializeAll()
	{
		DestroyAll();
		List<BaseBuffSO> buffs = BuffsData.ChoseRandomBuffAmmount(CardCount);
		for (int i = 0; i < buffs.Count; i++)
		{
			BuffCardUI card = Instantiate(BuffsData.BuffRarityCard[buffs[i].RareType],transform);
			card.Initialize(buffs[i]);
			Cards.Add(card);
			card.Button.onClick.AddListener(DeactivateParent);
		}
	}
	public void InitializeAllWithRarity(Rarity rarity)
	{
		DestroyAll();
		for (int i = 0; i < CardCount; i++)
		{
			BaseBuffSO buff = BuffsData.ChoseRarityBuff(rarity);
			BuffCardUI card = Instantiate(BuffsData.BuffRarityCard[buff.RareType],transform);
			card.Initialize(buff);
			Cards.Add(card);
			card.Button.onClick.AddListener(DeactivateParent);
		}
	}
	private void DeactivateParent()
	{
		UIManager.Instance.HidePanel(UIConstant.BuffPanel);
	}
}
