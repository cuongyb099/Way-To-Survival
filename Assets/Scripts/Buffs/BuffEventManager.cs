using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;
using Tech.Singleton;
using UnityEngine;
using Random = System.Random;

public class BuffEventManager : Singleton<BuffEventManager>
{
    [field:SerializeField,Range(0,1)] public float SpecialEventRate { get; set; } = .3f;
    [field:SerializeField,Range(0,1)] public float PositiveEventRate { get; set; } = .5f;
    [SerializeField] private ListOfBuffTypeSO positiveBuffsSO;
    [SerializeField] private ListOfBuffTypeSO negativeBuffsSO;
    public List<BaseBuffSO> PositiveBuffListSO { get; private set; }
    public List<BaseBuffSO> NegativeBuffListSO{ get; private set; }
    protected override void Awake()
    {
        base.Awake();
        PositiveBuffListSO = new List<BaseBuffSO>();
        NegativeBuffListSO = new List<BaseBuffSO>();
        GameEvent.OnChangeTimeOfDay += ChoseTimeOfDayBuff;
        GameEvent.OnStopCombatState += RemoveAllEffects;
    }

    private void OnDestroy()
    {
        GameEvent.OnChangeTimeOfDay -= ChoseTimeOfDayBuff;
        GameEvent.OnStopCombatState -= RemoveAllEffects;
    }

    private void ChooseBuffWithRate(float eventRate, float positiveRate)
    {
        if (UnityEngine.Random.value > eventRate) return;
        
        if (UnityEngine.Random.value > positiveRate)
        {
            AddNegativeEffect();
            return;
        }
        AddPositiveEffect();
    }

    private void AddPositiveEffect()
    {
        BaseBuffSO buff = positiveBuffsSO.ChooseRandomBuff();
        PositiveBuffListSO.Add(buff);
        buff.AddStatusEffect(GameManager.Instance.Player.GetComponent<StatsController>());
    }
    private void AddNegativeEffect()
    {
        BaseBuffSO buff = negativeBuffsSO.ChooseRandomBuff();
        NegativeBuffListSO.Add(buff);
        
        GameEvent.EnemySpawnEvent += x =>
        {
            buff.AddStatusEffect(x.GetComponent<StatsController>());
        };                                                                                                  
    }

    private void RemoveAllEffects()
    {
        for (int i = NegativeBuffListSO.Count-1; i>=0; --i)
        {
            GameEvent.EnemySpawnEvent -= x =>
            {
                NegativeBuffListSO[i].AddStatusEffect(x.GetComponent<StatsController>());
            };
            NegativeBuffListSO.RemoveAt(i);
        }
        for (int i = PositiveBuffListSO.Count-1; i>=0; --i)
        {
            GameManager.Instance.Player.GetComponent<StatsController>().RemoveEffect(PositiveBuffListSO[i]);
            PositiveBuffListSO.RemoveAt(i);
        }
    }
    private void ChoseTimeOfDayBuff(TimeOfTheDay timeOfTheDay)
    {
        RemoveAllEffects();
        switch (timeOfTheDay)
        {
            case TimeOfTheDay.MidNight:
                ChooseBuffWithRate(1,PositiveEventRate - .5f);
                break;
            case TimeOfTheDay.EarlyMorning:
                ChooseBuffWithRate(SpecialEventRate,PositiveEventRate-.1f);
                break;
            case TimeOfTheDay.Morning:
                ChooseBuffWithRate(SpecialEventRate,PositiveEventRate);
                break;
            case TimeOfTheDay.Noon:
                ChooseBuffWithRate(1,PositiveEventRate+.2f);
                break;
            case TimeOfTheDay.Afternoon:
                ChooseBuffWithRate(SpecialEventRate,PositiveEventRate+.1f);
                break;
            case TimeOfTheDay.Evening:
                ChooseBuffWithRate(SpecialEventRate,PositiveEventRate);
                break;
            default:
                return;
        }
    }
}
