using System;
using System.Collections;
using UnityEngine;

public class BasicBuffStatusEffect : BaseStatusEffect
{
    private BasicBuffSO basicBuffSO => Data as BasicBuffSO;
    private StatModifier statModifier;
    public BasicBuffStatusEffect(BasicBuffSO data, StatsController target, Action OnStart = null, Action onEnd = null, Action onActive = null) : base(data, target, OnStart, onEnd, onActive)
    {
        
    }

    protected override void HandleStart()
    {
        if (!basicBuffSO.Stackable)
        {
            statModifier = new StatModifier(basicBuffSO.Value, basicBuffSO.ModifierType);
            stats.AddModifier(basicBuffSO.StatType, statModifier);
        }
    }
    protected override void HandleOnUpdate()
    {
        
    }
    protected override void HandleEnd()
    {
        stats.RemoveModifier(basicBuffSO.StatType,statModifier);
    }

    public override void HandleStackChange()
    {
        stats.RemoveModifier(basicBuffSO.StatType,statModifier);
        statModifier = new StatModifier(basicBuffSO.Value * CurrentStack, basicBuffSO.ModifierType);
        stats.AddModifier(basicBuffSO.StatType, statModifier);
    }
}