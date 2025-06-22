using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AdvanceBuffSO", menuName = "Item/Buff/new AdvanceBuffSO")]
public class ShootingAddBuffSO : BaseBuffSO
{
	public BasicBuffSO BuffSO1;
	public int BuffAfter = 3;

	public override BaseStatusEffect AddStatusEffect(StatsController controller)
	{
		ShootingAddEffect buffEffect = new ShootingAddEffect(this, controller);
		controller.ApplyEffect(buffEffect);
		return buffEffect;
	}
	public override object[] GetValues()
	{
		return new object[]{BuffSO1.Value, BuffAfter, BuffSO1.Duration};
	}
}
