using System;
using System.Threading;
using DG.Tweening;
using Tech.Pooling;
using UnityEngine;

public class PoisonEffect : BaseStatusEffect
{
	protected static float eachTimeDealDamage = 0.5f;
	protected float damage;

	protected ParticleSystem.MainModule poisonParticle;
	protected IDamagable damgeable;
	private Tween _poisonTween;
    public PoisonEffect(StatusEffectSO data, StatsController target, float damage,
		Action OnStart = null, Action onEnd = null, Action onActive = null) : base(data, target, OnStart, onEnd, onActive)
    {
        OnInit(data, target);
        this.damage = damage;
        this.OnStart = OnStart;
        this.OnEnd = onEnd;
        this.OnActive = onActive;
    }

	public override void HandleStackChange()
	{
		
	}
	private float timer2;
	protected override void HandleOnUpdate()
	{
		timer2 += Time.deltaTime;

		if(timer2 > eachTimeDealDamage)
		{
			damgeable.Damage(new DamageInfo(null, damage));
			timer2 = 0;
		}
	}

	protected override void HandleEnd()
	{
        poisonParticle.loop = false;
	}

    protected override void HandleStart()
    {
		damgeable = stats.GetComponent<IDamagable>();
		var poisonData = Data as PoisonEffectSO;
		if(poisonData){
			var rotation = Quaternion.Euler(-90, 0, 0);
			var position = stats.transform.position + Vector3.up;
			var clone = ObjectPool.Instance.SpawnObject(poisonData.PoisonParticleEffect, position, rotation);
			clone.transform.SetParent(stats.transform);
			poisonParticle = clone.GetComponent<ParticleSystem>().main;
			poisonParticle.loop = true;
		}
    }

    public override void ChangeTarget(StatsController target)
    {
        base.ChangeTarget(target);
		damgeable = stats.GetComponent<IDamagable>();
    }
}
