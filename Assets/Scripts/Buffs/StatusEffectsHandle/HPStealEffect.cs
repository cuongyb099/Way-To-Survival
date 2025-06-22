using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPStealEffect : BaseStatusEffect
{
    private HPStealBuffSO HpStealBuffSoReal => Data as HPStealBuffSO;
	public HPStealEffect(HPStealBuffSO data, StatsController target, Action OnStart = null, Action onEnd = null, Action onActive = null) : base(data, target, OnStart, onEnd, onActive)
    {
		
    }
	protected override void HandleStart()
	{
		PlayerEvent.OnBulletDamageDealt += HPSteal;
		PlayerEvent.OnMeleeDamageDealt += HPSteal;
	}
    protected override void HandleOnUpdate()
    {

    }
    protected override void HandleEnd()
    {
		PlayerEvent.OnBulletDamageDealt -= HPSteal;
		PlayerEvent.OnMeleeDamageDealt -= HPSteal;
	}

    public override void HandleStackChange()
    {
        
    }
	public void HPSteal(float damage, IDamagable target)
	{
		stats.GetAttribute(AttributeType.Hp).Value += damage*(HpStealBuffSoReal.StealPercentage/100f);
	}
}