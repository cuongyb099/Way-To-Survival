using System;
using UnityEngine;

public static class PlayerEvent
{
    public static Action<float, float> OnHeathChange;
    public static Action<float, float> OnMaxHeathChange;
    public static Action<float, float> OnMaxManaChange;
    public static Action<float, float> OnManaChange;
	public static Action<AttributeType, float ,float> OnInitStatusBar;
	public static Action OnAttack;
	// Gun
	public static Action<WeaponBase> OnEquipWeapon;
	public static Action OnReload;
	public static Action OnChangeCap;
    public static Action<int> OnCashChange;
    public static Action<int> OnRecieveCash;
    public static Action<int> OnRecieveGunAmmo;
    // Damage
    public static Action<float, IDamagable> OnDamageDealt;
    public static Action<float, IDamagable> OnBulletDamageDealt;
    public static Action<float, IDamagable> OnFollowUpDamageDealt;
    public static Action<float, IDamagable> OnMeleeDamageDealt;


    public static Action OnInteractEnter;
    public static Action OnInteractExit;
}
