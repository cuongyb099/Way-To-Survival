using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageType
{
    Bullet,
    FollowUp,
    Melee,
}
public struct DamageInfo
{
    public GameObject Dealer;
    public float Damage;
	public bool IsCrit;
    public DamageType DamageType;

    public DamageInfo(GameObject dealer = null, float damage = 0, bool isCrit = false, DamageType dmgType = DamageType.Bullet)
    {
        Dealer = dealer;
        Damage = damage;
        IsCrit = isCrit;
        DamageType = dmgType;
    }
    
    public static DamageInfo GetDamageInfo(float multiplier, StatsController statsController, DamageType damageType,StatType statType = StatType.ATK)
    {
        if (!statsController) return new DamageInfo();

        GameObject dealer = statsController.gameObject;
        if (!statsController.Stats.TryGetValue(statType, out Stat mainStat)) 
            return new DamageInfo(dealer);
        
        float finalDamage = mainStat.Value * multiplier;
        if (!statsController.Stats.TryGetValue(StatType.CritRate, out Stat critRate) ||
            !statsController.Stats.TryGetValue(StatType.CritDamage, out Stat critDmg) ||
            damageType == DamageType.FollowUp)
            return new DamageInfo(dealer, finalDamage,dmgType: damageType);
        
        bool doesCrit = Random.value < (critRate.Value/100f);
        finalDamage *= (1f + (doesCrit ? (critDmg.Value/100f) : 0f));
        return new DamageInfo(dealer, finalDamage, doesCrit, damageType);
    }
}
