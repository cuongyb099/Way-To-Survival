using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAddEffect : BaseStatusEffect
{
	private int dem;
	private List<BaseStatusEffect> effects = new List<BaseStatusEffect>();
    private ShootingAddBuffSO ShootingAddBuffSoData => Data as ShootingAddBuffSO;
	public ShootingAddEffect(ShootingAddBuffSO data, StatsController target, Action OnStart = null, Action onEnd = null, Action onActive = null) : base(data, target, OnStart, onEnd, onActive)
    {
		
    }
	protected override void HandleStart()
	{
		PlayerEvent.OnAttack += AddSpeed;
	}
    protected override void HandleOnUpdate()
    {

    }
    protected override void HandleEnd()
    {
	    foreach (var x in effects)
	    {
		    stats.RemoveEffect(x);
	    }
		PlayerEvent.OnAttack -= AddSpeed;
	}

    public override void HandleStackChange()
    {
        
    }
	public void AddSpeed()
	{
		dem++;

		if (dem == ShootingAddBuffSoData.BuffAfter)
		{
			dem = 0;
			BaseStatusEffect buff = ShootingAddBuffSoData.BuffSO1.AddStatusEffect(stats);
			effects.Add(buff);
			buff.OnEnd += () => { effects.Remove(buff); };
		}
	}
}