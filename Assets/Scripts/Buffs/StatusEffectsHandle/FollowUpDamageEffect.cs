using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowUpDamageEffect : BaseStatusEffect
{
    private FollowUpDamageBuffSO RealData => Data as FollowUpDamageBuffSO;
	public FollowUpDamageEffect(FollowUpDamageBuffSO data, StatsController target, Action OnStart = null, Action onEnd = null, Action onActive = null) : base(data, target, OnStart, onEnd, onActive)
    {
		
    }
	protected override void HandleStart()
	{
		PlayerEvent.OnBulletDamageDealt += FollowUpAttack;
		PlayerEvent.OnMeleeDamageDealt += FollowUpAttack;
	}
    protected override void HandleOnUpdate()
    {

    }
    protected override void HandleEnd()
    {
		PlayerEvent.OnBulletDamageDealt -= FollowUpAttack;
		PlayerEvent.OnMeleeDamageDealt -= FollowUpAttack;
	}

    public override void HandleStackChange()
    {
        
    }
	public void FollowUpAttack(float damage, IDamagable target)
	{
		if(UnityEngine.Random.value > RealData.TriggerChance/100f) return;
		DamageHandler.Damage(target,DamageInfo.GetDamageInfo(RealData.DamagePercentage/100f,stats,damageType: DamageType.FollowUp));
	}
}