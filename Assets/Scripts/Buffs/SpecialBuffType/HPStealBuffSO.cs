using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HPStealBuffSO", menuName = "Item/Buff/new HPStealBuffSO")]
public class HPStealBuffSO : BaseBuffSO
{
	public float StealPercentage;
	public override BaseStatusEffect AddStatusEffect(StatsController controller)
	{
		HPStealEffect buffEffect = new HPStealEffect(this, controller);
		controller.ApplyEffect(buffEffect);
		return buffEffect;
	}
	public override object[] GetValues()
	{
		return new object[]{StealPercentage};
	}
}
