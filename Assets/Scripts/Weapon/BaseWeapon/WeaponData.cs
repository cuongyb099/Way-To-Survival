
using System;
using System.Collections.Generic;
using KatInventory;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class WeaponData : ItemGOData
{
    [JsonIgnore]
    public WeaponBaseSO WeaponSO => (WeaponBaseSO)StaticData;

    [JsonProperty("WeaponLevel", Order = 2)] 
    public int WeaponLevel { get; protected set; }
    [JsonProperty("WeaponSpeed", Order = 3)] 
    public UpgradableFloat ShootingSpeed{ get; private set; }
    [JsonProperty("WeaponDamage", Order = 4)] 
    public UpgradableFloat Damage{ get; private set; }
    [JsonProperty("WeaponWeight", Order = 5)] 
    public UpgradableFloat Weight{ get; private set; }
    public WeaponData(WeaponBaseSO staticData, int quantity, GameObject go) : base(staticData, quantity, go)
    {
        WeaponLevel = 0;
        ShootingSpeed = new UpgradableFloat(Random.Range(staticData.ShootingSpeed*0.8f,staticData.ShootingSpeed*1.1f));
        Damage = new UpgradableFloat(Random.Range(staticData.Damage*0.8f,staticData.Damage*1.1f));
        Weight = new UpgradableFloat(Random.Range(staticData.Weight*0.8f,staticData.Weight*1.1f));
    }

    public virtual void UpgradeWeapon()
    {
        if(WeaponLevel>=15) return;
        ++WeaponLevel;
        ShootingSpeed.UpgradeNegative();
        Damage.Upgrade();
    }
    public virtual void OnEquip(PlayerController playerController)
    {
        WeaponSO.BuffUnlockByLevel.ApplyBuffToLevel(WeaponLevel,playerController.Stats);
    }

    public virtual void OnUnequip(PlayerController playerController)
    {
        
    }

    [Serializable]
    public class UpgradableFloat
    {
        [JsonIgnore]
        protected static float UpgradeValue = 0.02f;
        [JsonIgnore]
        public float Value
        {
            get
            {
                if (isDirty)
                {
                    value = AddOnValue + BaseValue;
                    isDirty = false;
                }
                return value;
            }
        }
        [JsonIgnore]
        private float value;
        [JsonProperty("AddOnValue")] 
        public float AddOnValue { get; private set; } = 0f;
        [JsonProperty("BaseValue")]
        public float BaseValue { get; private set; } = 0f;

        private bool isDirty = true;
        public UpgradableFloat()
        {
        }
        public UpgradableFloat(float baseValue = 0f)
        {
            AddOnValue = 0f;
            BaseValue = baseValue;
        }
        public UpgradableFloat(float addOnValue, float baseValue)
        {
            AddOnValue = addOnValue;
            BaseValue = baseValue;
        }

        public float GetUpgradableValue()
        {
            return BaseValue * (1f+ UpgradeValue) + AddOnValue;
        }
        public void Upgrade()
        {
            AddOnValue += BaseValue * UpgradeValue;
        }
        public void UpgradeNegative()
        {
            AddOnValue -= BaseValue * UpgradeValue;
        }
    }
}
