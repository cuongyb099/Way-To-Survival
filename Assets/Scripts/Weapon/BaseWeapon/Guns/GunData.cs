
using Newtonsoft.Json;
using UnityEngine;
public class GunData : WeaponData
{
    [JsonIgnore]
    public GunBaseSO GunSO => (GunBaseSO)StaticData;
    [JsonProperty("GunAim", Order = 6)] 
    public UpgradableFloat Aim{ get; private set; }
    [JsonProperty("GunSpread", Order = 7)] 
    public UpgradableFloat SpreadMax{ get; private set; }
    [JsonProperty("GunRecoil", Order = 8)] 
    public UpgradableFloat Recoil{ get; private set; }
    public GunData(GunBaseSO staticData, int quantity, GameObject go) : base(staticData, quantity, go)
    {
        Aim = new UpgradableFloat(Random.Range(staticData.Aim*0.8f,staticData.Aim*1.1f));
        SpreadMax = new UpgradableFloat(Random.Range(staticData.SpreadMax*0.8f,staticData.SpreadMax*1.1f));
        Recoil = new UpgradableFloat(Random.Range(staticData.Recoil*0.8f,staticData.Recoil*1.1f));
    }

    public override void UpgradeWeapon()
    {
        if(WeaponLevel>=15) return;
        ++WeaponLevel;
        ShootingSpeed.UpgradeNegative();
        Damage.Upgrade();
        Recoil.UpgradeNegative();
    }
}
