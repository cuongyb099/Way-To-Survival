using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using AYellowpaper.SerializedCollections;
using System;
using System.Threading;
using KatInventory;

[CreateAssetMenu(fileName = "BuffListSO", menuName = "Item/Buff/new BuffListSO")]
public class BuffListSO : ScriptableObject
{
	[SerializedDictionary("Rarity", "Buffs")]
	[SerializeField] public SerializedDictionary<Rarity, ListOfBuffTypeSO> Buffs = new();
	[SerializedDictionary("Rarity", "Data")]
	[SerializeField] public SerializedDictionary<Rarity, BuffCardUI> BuffRarityCard = new();
	[SerializedDictionary("Rarity", "Rate")]
	[SerializeField] public SerializedDictionary<Rarity, float> BuffRarityRate = new();
	public BaseBuffSO ChoseRandomRarityBuff()
    {
        float curSum = 0;
        float rdnNum = UnityEngine.Random.value* calSumNumber();
		foreach (var key in BuffRarityRate.Keys)
		{
            curSum += BuffRarityRate[key];
            if (rdnNum > curSum) continue;
            return Buffs[key].ChooseRandomBuff();
		}
        return null;
    }
    private float calSumNumber()
    {
        float sum = 0;
        foreach(var key in BuffRarityRate.Keys)
        {
            sum += BuffRarityRate[key];
        }
        return sum;
    }
	public List<BaseBuffSO> ChoseRandomBuffAmmount(int n)
	{
        List<BaseBuffSO> l = new List<BaseBuffSO>();
        int count = 0;
        while (l.Count < n && count <999)
        {
            count++;
            BaseBuffSO t = ChoseRandomRarityBuff();
            if (!l.Contains(t))
            {
                if (!t.Stackable && GameManager.Instance.Player.Stats.HasEffect(t)) continue;
                l.Add(t);
            }
        }

        return l;
	}
}
