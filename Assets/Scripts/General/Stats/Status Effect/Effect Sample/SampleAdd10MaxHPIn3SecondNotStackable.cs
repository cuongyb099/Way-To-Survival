#if UNITY_EDITOR

using System;
using UnityEngine;

public class SampleAdd10MaxHPIn3SecondNotStackable : BaseStatusEffect
{
    protected StatModifier modifier;
    
    public SampleAdd10MaxHPIn3SecondNotStackable(StatusEffectSO data, StatsController target, StatModifier modifier,
        Action OnStart = null, Action onEnd = null, Action onActive = null) : base(data, target, OnStart, onEnd, onActive)
    {
        this.modifier = modifier;
    }

    public override void HandleStackChange()
    {
        
    }

    protected override void HandleStart()
    {
        stats.AddModifier(StatType.MaxHP, modifier);   
    }

    protected override void HandleOnUpdate()
    {
        
    }

    protected override void HandleEnd()
    {
        stats.RemoveModifier(StatType.MaxHP, modifier);
    }

    public override BaseStatusEffect Clone()
    {
        return (SampleAdd10MaxHPIn3SecondNotStackable)this.MemberwiseClone();
    }
}
#endif
