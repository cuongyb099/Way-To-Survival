using System;
using UnityEngine;

[Serializable]
public class AttributeItem
{
    [Range(0,1)]
    [SerializeField] private float _startPercent = 1f;
    [SerializeField] private float _minValue;
    [SerializeField] private StatType _maxValue;
    
    public float MinValue => _minValue;
    public StatType MaxValue => _maxValue;
    public float StartPercent => _startPercent;
}