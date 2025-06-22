using System.Collections;
using System.Collections.Generic;
using Unity.Entities.UniversalDelegates;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffSO", menuName = "Item/Buff/new BuffSO")]
public class BasicBuffSO : BaseBuffSO
{
    [Header("Basic Buff Data")]
	public StatType StatType;
    public StatModType ModifierType;
	public float Value;

	public override BaseStatusEffect AddStatusEffect(StatsController controller)
	{
		BasicBuffStatusEffect buffEffect = new BasicBuffStatusEffect(this, controller);
		controller.ApplyEffect(buffEffect);
		return buffEffect;
	}

	public override object[] GetValues()
	{
		return new object[]{Mathf.Abs(Value)};
	}
}
