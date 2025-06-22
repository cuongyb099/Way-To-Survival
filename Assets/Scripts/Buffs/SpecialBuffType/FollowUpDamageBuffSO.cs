using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FollowUpDamageBuffSO", menuName = "Item/Buff/new FollowUpDamageBuffSO")]
public class FollowUpDamageBuffSO : BaseBuffSO
{
	public float DamagePercentage;
	public float TriggerChance;
	public override BaseStatusEffect AddStatusEffect(StatsController controller)
	{
		FollowUpDamageEffect buffEffect = new FollowUpDamageEffect(this, controller);
		controller.ApplyEffect(buffEffect);
		return buffEffect;
	}
	public override object[] GetValues()
	{
		return new object[]{DamagePercentage, TriggerChance};
	}
}
