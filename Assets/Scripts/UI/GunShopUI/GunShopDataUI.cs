using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using KatInventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GunShopDataUI : MonoBehaviour
{
    [field:SerializeField] public GameObject ShowWeaponPanel { get; private set; }
    [field:SerializeField] public GameObject NoWeaponPanel { get; private set; }
    public TextMeshProUGUI GunName;
    public TextMeshProUGUI GunPrice;
    public TextMeshProUGUI Capacity;
    public TextMeshProUGUI GunLevel;
    public Image GunImage;
    public Slider DamageSlider;
    public Slider RecoilSlider;
    public Slider AimSlider;
    public Slider RPMSlider;
    public Slider WeightSlider;
    public Slider CapacitySlider;
    
    public float AnimTime = 0.25f;
    public WeaponData GunData { get; private set; }

    public void ChangeGun(WeaponData gunData)
    {
        if (gunData == null)
        {
            GunData = null;
            ShowWeaponPanel.SetActive(false);
            NoWeaponPanel.SetActive(true);
            ResetSliderValues();
            return;
        }

        GunData = gunData;
        var x = (GunData)gunData;
        GunName.text = x.GunSO.Name.GetLocalizedString();
        GunPrice.text = $"<color=#1BDF00>{x.GunSO.BuyPrice}$</color>";
        Capacity.text = x.GunSO.MaxCapacity.ToString();
        GunLevel.text = "+" + x.WeaponLevel.ToString();
        GunImage.sprite = x.GunSO.Icon;
        ShowWeaponPanel.SetActive(true);
        NoWeaponPanel.SetActive(false);

        DOVirtual.Float(DamageSlider.value, x.Damage.Value / 15f, AnimTime, (x) => { DamageSlider.value = x;}).SetUpdate(true);
        //15f is the maximum spread angle
        DOVirtual.Float(RecoilSlider.value, x.Recoil.Value, AnimTime, (x) => { RecoilSlider.value = x;}).SetUpdate(true);
        DOVirtual.Float(AimSlider.value, x.Aim.Value/30f, AnimTime, (x) => { AimSlider.value = x;}).SetUpdate(true);
        //1min:1000RPM=0.06
        DOVirtual.Float(RPMSlider.value, 0.06f/x.ShootingSpeed.Value, AnimTime, (x) => { RPMSlider.value = x;}).SetUpdate(true);
        DOVirtual.Float(WeightSlider.value, x.Weight.Value/3f, AnimTime, (x) => { WeightSlider.value = x;}).SetUpdate(true);
        DOVirtual.Float(CapacitySlider.value, x.GunSO.MaxCapacity / 100f, AnimTime, (x) => { CapacitySlider.value = x;}).SetUpdate(true);
    }

    public void ResetSliderValues()
    {
        DamageSlider.value = 0f;
        RecoilSlider.value = 0f;
        AimSlider.value = 0f;
        RPMSlider.value = 0f;
        WeightSlider.value = 0f;
        CapacitySlider.value = 0f;
    }
}
