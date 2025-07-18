using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KatInventory;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class WeaponShopDataUI : MonoBehaviour
{
    [field:SerializeField] public GameObject ShowWeaponPanel { get; private set; }
    [field:SerializeField] public GameObject NoWeaponPanel { get; private set; }
    public TextMeshProUGUI WeaponName;
    public TextMeshProUGUI WeaponPrice;
    public TextMeshProUGUI Capacity; 
    public TextMeshProUGUI WeaponLevel;
    public Image WeaponImage;
    public WeaponSliderStats DamageSlider;
    public WeaponSliderStats RecoilSlider;
    public WeaponSliderStats AimSlider;
    public WeaponSliderStats RPMSlider;
    public WeaponSliderStats WeightSlider;
    public WeaponSliderStats CapacitySlider;
    
    public float AnimTime = 0.25f;
    public WeaponData WeaponData { get; private set; }

    public void ChangeGun(WeaponData weaponData)
    {
        if (weaponData == null)
        {
            WeaponData = null;
            ShowWeaponPanel.SetActive(false);
            NoWeaponPanel.SetActive(true);
            ResetSliderValues();
            return;
        }

        WeaponData = weaponData;
        var x = (GunData)weaponData;
        
        WeaponName.text = x.GunSO.Name.GetLocalizedString();
        WeaponPrice.text = $"<color=#1BDF00>{x.GunSO.BuyPrice}$</color>";
        WeaponImage.sprite = x.GunSO.Icon;
        ShowWeaponPanel.SetActive(true);
        NoWeaponPanel.SetActive(false);
        
        UpdateGunStats();
    }

    public void UpdateGunStats()
    {
        var x = (GunData)WeaponData;
        if(x == null) return;
        Capacity.text = x.GunSO.MaxCapacity.ToString();
        WeaponLevel.text = "+" + x.WeaponLevel;
        
        DamageSlider.ChangeStat(x.Damage.Value,0f,15f,"F2");
        RecoilSlider.ChangeStat(x.Recoil.Value,0f,1f,"F2");
        AimSlider.ChangeStat(x.Aim.Value,0f,30f,"F1");
        RPMSlider.ChangeStat((int)(60/x.ShootingSpeed.Value),0f,1000f,"");
        WeightSlider.ChangeStat(x.Weight.Value,0f,10f,"F1");
        CapacitySlider.ChangeStat((int)x.GunSO.MaxCapacity,0f,200f,"");
        
    }

    public void ResetSliderValues()
    {
        DamageSlider.Slider.value = 0f;
        RecoilSlider.Slider.value = 0f;
        AimSlider.Slider.value = 0f;
        RPMSlider.Slider.value = 0f;
        WeightSlider.Slider.value = 0f;
        CapacitySlider.Slider.value = 0f;
    }
}
