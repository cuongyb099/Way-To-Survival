using System.Collections;
using System.Collections.Generic;
using KatInventory;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public class BaseBuffSO : StatusEffectSO
{
    [Header("Buff Data")]
	public Rarity RareType;

	public override BaseStatusEffect AddStatusEffect(StatsController controller)
	{
		return null;
	}
	public virtual object[] GetValues()
	{
		return null;
	}
}
