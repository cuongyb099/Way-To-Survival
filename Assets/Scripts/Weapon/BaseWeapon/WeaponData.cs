
using System;
using System.Collections.Generic;
using JsonSubTypes;
using KatInventory;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;
// [JsonConverter(typeof(JsonSubtypes))]
// [JsonSubtypes.KnownSubTypeWithProperty(typeof(GunData), "GunAim")]
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
    [JsonConstructor]
    public WeaponData(string ID, int quantity, int weaponLevel, UpgradableFloat shootingSpeed, 
        UpgradableFloat damage, UpgradableFloat weight)  : base(ID, quantity)
    {
        WeaponLevel = weaponLevel;
        ShootingSpeed = shootingSpeed;
        Damage = damage;
        Weight = weight;
    }
    public WeaponData(WeaponBaseSO staticData, int quantity) : base(staticData, quantity)
    {
        WeaponLevel = 0;
        ShootingSpeed = new UpgradableFloat(Random.Range(staticData.ShootingSpeed*0.8f,staticData.ShootingSpeed*1.1f));
        Damage = new UpgradableFloat(Random.Range(staticData.Damage*0.8f,staticData.Damage*1.1f));
        Weight = new UpgradableFloat(Random.Range(staticData.Weight*0.8f,staticData.Weight*1.1f));
    }

    public virtual void UpgradeWeapon()
    {
        ++WeaponLevel;
        ShootingSpeed.UpgradeNegative(WeaponLevel);
        Damage.Upgrade(WeaponLevel);
    }
    [Serializable]
    public class UpgradableFloat
    {
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

        public float GetUpgradableValue(int level)
        {
            return BaseValue * (1f+ GameDataManager.Instance.UpgradeCurve.Evaluate(level)) + AddOnValue;
        }
        public void Upgrade(int level)
        {
            AddOnValue += BaseValue * GameDataManager.Instance.UpgradeCurve.Evaluate(level);
            isDirty = true;
        }
        public void UpgradeNegative(int level)
        {
            AddOnValue -= BaseValue * GameDataManager.Instance.UpgradeCurve.Evaluate(level);
            isDirty = true;
        }
    }
}
