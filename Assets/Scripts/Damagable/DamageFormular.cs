using UnityEngine;

public static class DamageFormular
{
    public static void Damage(DamageInfo info, StatsController Stats)
    {
        float finalDamage = Mathf.Clamp(info.Damage - Stats.GetStat(StatType.DEF).Value, 0, int.MaxValue);
        Stats.GetAttribute(AttributeType.Hp).Value -= finalDamage;
    }
}