
using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[Serializable]
public class BuffUnlockByLevel
{
    [SerializedDictionary("Active Level", "Data")]
    [SerializeField] private SerializedDictionary<int, BaseBuffSO> _buffLevels = new();

    public void ApplyBuffToLevel(int level, StatsController stats)
    {
        for (int i =1; i<=level; ++i)
        {
            if (_buffLevels.TryGetValue(i, out BaseBuffSO statusEffect))
            {
                statusEffect.AddStatusEffect(stats);
            }
        }
    }
}
