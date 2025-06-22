using UnityEngine;
using AYellowpaper.SerializedCollections;
using System.Collections.Generic;
using Tech.Logger;
using System;

[CreateAssetMenu(fileName = "M_StatsHolder", menuName = "Stats/M_StatsHolder")]
public class StatsHolderSO : ScriptableObject
{
   [SerializedDictionary("Stat Type", "Data")]
   [SerializeField] private SerializedDictionary<StatType, StatItem> _statItems = new();
   [SerializedDictionary("Attribute Type", "Data")]
   [SerializeField] private SerializedDictionary<AttributeType, AttributeItem> _attributeItems = new();

   public Dictionary<StatType, StatItem> StatItems => _statItems;
   public Dictionary<AttributeType, AttributeItem> AttributeItems => _attributeItems;

   public int AttributesCount() => _attributeItems.Count;
   public int StatsCount() => _statItems.Count;

   public StatItem GetStat(StatType type)
   {
      if(_statItems.TryGetValue(type, out StatItem value))
      {
         return value;
      }
      
      LogCommon.LogWarning($"{type} not Found");
      return default;
   }

   public AttributeItem GetAttribute(AttributeType type)
   {
      if(_attributeItems.TryGetValue(type, out AttributeItem value))
      {
         return value;
      }
      
      LogCommon.LogWarning($"{type} not Found");
      return default;
   }
}
