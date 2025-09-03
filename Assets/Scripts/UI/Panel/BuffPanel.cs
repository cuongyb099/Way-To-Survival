using System.Collections;
using System.Collections.Generic;
using KatInventory;
using UnityEngine;

public class BuffPanel : FadeBlurPanel
{
    public static BuffPanel Instance => instance;
    private static BuffPanel instance;
    [Header("Buff Data")]
    [SerializeField] private BuffListSO BuffsData;
	[SerializeField] private int CardCount = 3;
	[SerializeField] private Transform CardUIHolder;
	private List<BuffCardUI> Cards;
    protected override void OnAwake()
    {
        base.OnAwake();
        if(instance != null) Destroy(gameObject);
        else instance = this;
        
        Cards = new List<BuffCardUI>();
    }
    public void DestroyAll()
    {
    	for(int i = Cards.Count - 1; i >= 0; i--)
    	{
    		BuffCardUI card = Cards[i];
    		Cards.RemoveAt(i);
    		card.Button.onClick.RemoveListener(Hide);
    		Destroy(card.gameObject);
    	}
    }
    public void InitializeAllRandom()
    {
	    Show();
    	DestroyAll();
    	List<BaseBuffSO> buffs = BuffsData.ChoseRandomBuffAmmount(CardCount);
    	for (int i = 0; i < buffs.Count; i++)
    	{
    		BuffCardUI card = Instantiate(BuffsData.BuffRarityCard[buffs[i].RareType],CardUIHolder);
    		card.Initialize(buffs[i]);
    		Cards.Add(card);
    		card.Button.onClick.AddListener(Hide);
    	}
    }
    public void InitializeAllWithRarity(Rarity rarity)
    {
	    Show();
    	DestroyAll();
	    List<BaseBuffSO> buffs = BuffsData.ChoseRandomBuffAmmountWithRarity(CardCount,rarity);
	    for (int i = 0; i < buffs.Count; i++)
	    {
		    BuffCardUI card = Instantiate(BuffsData.BuffRarityCard[buffs[i].RareType],CardUIHolder);
		    card.Initialize(buffs[i]);
		    Cards.Add(card);
		    card.Button.onClick.AddListener(Hide);
	    }
    }
}
