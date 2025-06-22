using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public PlayerController PlayerController { get; private set; }
	private void Awake()
	{
		PlayerController = GetComponentInParent<PlayerController>();
	}
	//Weapons
	public void EnableShooting()
	{
		PlayerController.Animator.SetBool("SwitchWeapon", false);
		PlayerController.AfterSwitching();
	}
	public void DisableShooting()
	{
		PlayerController.Animator.SetBool("SwitchWeapon", true);
		PlayerController?.BeforeSwitching();
	}
	//Gun
	public void AfterReload()
	{
		
		PlayerController.Animator.SetBool("ReloadGun", false);
		PlayerController.AfterReload();
	}
	public void BeforeReload()
	{
		PlayerController.Animator.SetBool("ReloadGun", true);
		PlayerController?.BeforeSwitching();
	}
	public void DropMagazine()
	{
		GunBase gun = (GunBase)PlayerController.CurrentWeapon;
		gun?.DropMagazine();
	}

	public void DropShell()
	{
		GunBase gun = (GunBase)PlayerController.CurrentWeapon;
		gun?.DropShell();
	}

	public void TakeMagazine()
	{
		GunBase gun = (GunBase)PlayerController.CurrentWeapon;
		gun?.TakeMagazine();
	}

	public void PutInMagazine()
	{
		GunBase gun = (GunBase)PlayerController.CurrentWeapon;
		gun?.PutInMagazine();
	}
	//Melee
	public void DamageMelee()
	{
		MeleeBase weapon = (MeleeBase)PlayerController.CurrentWeapon;
		weapon?.DealDamage();
	}
	//Sounds
	public void CockingSound()
	{
		GunBase gun = (GunBase)PlayerController.CurrentWeapon;
		AudioManager.Instance.PlaySound(gun?.GunData.GunSO.CockingSound,volumeType: SoundVolumeType.SOUNDFX_VOLUME);
	}
	public void MagSoundIn()
	{
		GunBase gun = (GunBase)PlayerController.CurrentWeapon;
		AudioManager.Instance.PlaySound(gun.GunData.GunSO.MagSoundIn,volumeType: SoundVolumeType.SOUNDFX_VOLUME);
	}
	public void MagSoundOut()
	{
		GunBase gun = (GunBase)PlayerController.CurrentWeapon;
		AudioManager.Instance.PlaySound(gun.GunData.GunSO.MagSoundOut,volumeType: SoundVolumeType.SOUNDFX_VOLUME);
	}
}
