using DG.Tweening;
using ResilientCore;
using System;
using System.Collections;
using KatInventory;
using Tech.Pooling;
using UnityEditor.Rendering;
using UnityEngine;

public class GunBase : WeaponBase
{
	public GunData GunData => (GunData)Data;
	[field: SerializeField] public Transform ShootPoint { get; private set; }
	[field: SerializeField] public Transform ShellDropPoint { get; private set; }
	[field: SerializeField] public GameObject MagObject { get; private set; }
	public bool IsFullCap { get { return Stats.GetAttribute(AttributeType.Bullets).Value == Stats.GetStat(StatType.MaxBulletCap).Value; } }
	public bool IsEmpty { get { return Stats.GetAttribute(AttributeType.Bullets).Value == 0; } }
	public float GunAccuracy
	{
		get { return GunRecoil * GunData.SpreadMax.Value * playerController.Stats.GetStat(StatType.MaxSpreadReduce).Value; }
	}
	public float GunRecoil { get; protected set; } = 0f;
	public StatsController Stats { get; protected set; }
	
	private TriggerHandler triggerHandler;
	protected bool gunReloadable = true;
	protected override void Awake()
	{
		base.Awake();
		Stats = GetComponent<StatsController>();
		triggerHandler = GetComponent<TriggerHandler>();
	}
	public override void Initialize()
	{
		SetBulletCap();
		Stats.GetAttribute(AttributeType.Bullets).SetValueToMax();
	}
	protected override void OnEnable()
	{
		base.OnEnable();
		InputEvent.OnInputReloadGun += ReloadGun;
		MagObject.SetActive(true);
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		InputEvent.OnInputReloadGun -= ReloadGun;
	}
	public override void OnSwitchOut()
	{
		ResetRecoil();
		playerController.DisableLineRenderer();
		gunReloadable = false;
	}
	public override void OnSwitchIn()
	{
		playerController.EnableLineRenderer();
		gunReloadable = true;
	}

	private Tween temp;
	protected override void Update()
	{
		Vector2 rotateInput = PlayerInput.Instance.ShootStickInput;
		
		//Controller
		if (PlayerInput.Instance.IsAttackInput)
		{
			Shoot();
			return;
		}
		//Mobile
		if (rotateInput.magnitude > 0.875f)
		{
			if (!GunData.GunSO.ReleaseToShoot) { Shoot(); }
			trigger = true;
		}
		else
		{
			if (IsEmpty)
			{
				ReloadGun();
			}

			trigger = false;
		}
	}
	public virtual void ResetRecoil()
	{
		temp.Kill();
		temp = DOVirtual.Float(GunRecoil, 0, GunData.GunSO.RecoilResetTime, (x) => { GunRecoil = x; });
	}
	private void Rotate_canceled()
	{
		if (GunData.GunSO.ReleaseToShoot && trigger) Shoot();
	}
	public override void Shoot()
	{
		if (!ShootAble ||
			Stats.GetAttribute(AttributeType.Bullets).Value <= 0 ||
			!repeatAble ||
			triggerHandler.IsTriggered) return;
		repeatAble = false;
		temp.Kill();
		Stats.GetAttribute(AttributeType.Bullets).Value--;
		PlayerEvent.OnAttack?.Invoke();
		DOVirtual.DelayedCall(GunData.ShootingSpeed.Value/playerController.Stats.GetStat(StatType.ATKSpeed).Value, 
			() => { repeatAble = true; ResetRecoil(); });
		GunSoundPlay();
		BulletInstantiate();
		GunRecoilUpdate();
	}
	public void ReloadGun()
	{
		if (!gunReloadable || IsFullCap || (GunData.GunSO.WeaponType != WeaponType.Pistol && playerController.Stats.GetAttribute(AttributeType.HoldingBullets).Value == 0)) return;
		playerController.DisableLineRenderer();
		ShootAble = false;
		playerController.Animator.SetBool("ReloadGun", true);
		ResetRecoil();
	}

	protected override void WeaponSoundPlay()
	{
		base.WeaponSoundPlay();
		if(GunData.GunSO.TailSound)
			AudioManager.Instance.PlaySound(GunData.GunSO.TailSound,volumeType: SoundVolumeType.SOUNDFX_VOLUME);
	}

	private void GunSoundPlay()
	{
		AudioManager.Instance.PlaySound(GunData.GunSO.AttackSounds.ToArray(),volumeType: SoundVolumeType.SOUNDFX_VOLUME);
		if(GunData.GunSO.TailSound)
			AudioManager.Instance.PlaySound(GunData.GunSO.TailSound,volumeType: SoundVolumeType.SOUNDFX_VOLUME);
	}
	
	public virtual void GunRecoilUpdate()
	{
		GunRecoil += GunData.Recoil.Value* playerController.Stats.GetStat(StatType.RecoilReduce).Value;
		if (GunRecoil >= 1) { GunRecoil = 1f; return;}
		if (GunRecoil < 0) { GunRecoil = 0f; return;}
	}
	public virtual void BulletInstantiate()
	{
		GameObject a = ObjectPool.Instance.SpawnObject(GunData.GunSO.BulletPrefab, ShootPoint.position, transform.rotation, PoolType.GameObject);
		Bullet bullet = a.GetComponent<Bullet>();

		bullet.InitBullet(ShootPoint.position, GunAccuracy, DamageInfo.GetDamageInfo(GunData.Damage.Value,playerController.Stats, DamageType.Bullet));
	}
	public void SetBulletCap(float mul=1)
	{
		Stats.GetStat(StatType.MaxBulletCap).BaseValue = (int)(GunData.GunSO.MaxCapacity * mul);
	}

	private Attribute bulletSource,att;
	public void ReloadBullet()
	{
		att = Stats.GetAttribute(AttributeType.Bullets);
		if(GunData.GunSO.WeaponType != WeaponType.Pistol)
		{
			bulletSource = playerController.Stats.GetAttribute(AttributeType.HoldingBullets);
			
			if (bulletSource.Value >= att.MaxValue- att.Value)
			{
				bulletSource.Value -= (att.MaxValue- att.Value);
				att.SetValueToMax();
			}
			else
			{
				att.Value += bulletSource.Value;
				bulletSource.Value = 0;
			}
		}
		else
		{
			att.SetValueToMax();
		}
	}
	public void SetBulletToMax()
	{
		Stats.GetAttribute(AttributeType.Bullets).SetValueToMax();
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(base.GetHashCode(), GunData);
	}
	//Animation Methods
	public void DropMagazine()
	{
		GameObject mag = ObjectPool.Instance.SpawnObject(GunData.GunSO.MagPrefab,MagObject.transform.position,MagObject.transform.rotation, PoolType.GameObject);
		Rigidbody rb = mag.GetComponentInChildren<Rigidbody>();
		rb.velocity = playerController.Rigidbody.velocity;
		MagObject.SetActive(false);
	}

	public void DropShell()
	{
		GameObject shell = ObjectPool.Instance.SpawnObject(GunData.GunSO.ShellPrefab,ShellDropPoint.transform.position,transform.rotation* Quaternion.Euler(UnityEngine.Random.Range(0,60),10,0), PoolType.GameObject);
		Rigidbody rb = shell.GetComponent<Rigidbody>();
		rb.velocity = playerController.Rigidbody.velocity;
		rb.AddForce(Quaternion.Euler(0,-90,0)*shell.transform.forward*2f, ForceMode.VelocityChange);
	}

	public void TakeMagazine()
	{
		
	}

	public void PutInMagazine()
	{
		MagObject.SetActive(true);
	}
	
}
