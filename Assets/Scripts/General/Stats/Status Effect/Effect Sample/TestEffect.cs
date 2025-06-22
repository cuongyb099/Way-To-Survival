#if UNITY_EDITOR

using System;
using UnityEngine;

public class TestEffect : MonoBehaviour
{
    public StatusEffectSO TestData;
    public StatusEffectSO TestData2;
    private StatsController stats;
    private BaseStatusEffect effect1;
    private BaseStatusEffect effect2;
    private void Awake()
    {
        stats = GetComponent<StatsController>();
        effect1 = new SampleAddMaxHPIn3SecondStackable(TestData, stats, new StatModifier(10, StatModType.Flat));
        effect2 = new SampleAdd10MaxHPIn3SecondNotStackable(TestData2, stats, new StatModifier(10, StatModType.Flat));
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            SampleAdd10MaxHpIn3Second();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            SampleAdd10MaxHpStackable();
        }
    }

    [ContextMenu("Sample Add 10 MaxHp In 3 Second Stackable")]
    public void SampleAdd10MaxHpIn3Second()
    {
        stats.ApplyEffect(effect1);
    }    
    
    [ContextMenu("Sample Add 10 MaxHp In 3 Second Not Stackable")]
    public void SampleAdd10MaxHpStackable()
    {
        stats.ApplyEffect(effect2);
    }    
}

#endif
