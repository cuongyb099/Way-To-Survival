using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class GunStatusUI : MonoBehaviour
{
	private GunBase gun;
    private Attribute gunAmmo;
    private Attribute holdingAmmo;
    public Image GunIcon;
	public TextMeshProUGUI TextAmmo;
	public TextMeshProUGUI TextGunName;
	private LocalizedString CurrentGunName;
	private void Awake()
	{
        PlayerEvent.OnEquipWeapon += ChangeGun;
        PlayerEvent.OnAttack += UpdateGunAmmo;
		PlayerEvent.OnReload += UpdateGunAmmo;
		PlayerEvent.OnChangeCap += UpdateGunAmmo;
		LocalizationSettings.SelectedLocaleChanged += UpdateGunName;
	}

	private void Start()
	{
		ChangeGun(GameManager.Instance.Player.CurrentWeapon);
	}

	private void OnDestroy()
	{
		PlayerEvent.OnEquipWeapon -= ChangeGun;
		PlayerEvent.OnAttack -= UpdateGunAmmo;
		PlayerEvent.OnReload -= UpdateGunAmmo;
		PlayerEvent.OnChangeCap -= UpdateGunAmmo;
		LocalizationSettings.SelectedLocaleChanged -= UpdateGunName;
	}
	public void UpdateGunAmmo()
    {
	    if (gunAmmo == null)
		    TextAmmo.text = "∞";
	    else
	    {
		    string txt = (gun.GunData.GunSO.WeaponType is WeaponType.Pistol)? "∞": ((int)(holdingAmmo.Value/GunBase.bulletMultiplier[gun.GunData.GunSO.WeaponType])).ToString();
		    TextAmmo.text = $"{gunAmmo.Value} <size=70%><voffset=4.86135><color=#FFFFFF8C>/{txt}</color></voffset></size>";
	    }
    }
	public void ChangeGun(WeaponBase weapon)
	{
		if (weapon.WeaponData.WeaponSO.WeaponType is not (WeaponType.Knife or WeaponType.SpecialWeapon))
		{
			gun = (GunBase)weapon;
			gunAmmo = gun.Stats.GetAttribute(AttributeType.Bullets);
			holdingAmmo = GameManager.Instance.Player.Stats.GetAttribute(AttributeType.HoldingBullets);
		}
		else
		{
			gun = null;
			gunAmmo = null;
		}
        GunIcon.sprite = weapon.WeaponData.WeaponSO.Icon;
        CurrentGunName = weapon.WeaponData.WeaponSO.Name;
		TextGunName.text = CurrentGunName.GetLocalizedString();
        UpdateGunAmmo();
    }

	private void UpdateGunName(Locale locale)
	{
		TextGunName.text = CurrentGunName.GetLocalizedString();
	}
}
