using System;
using UnityEngine;

[Serializable]
public class StatItem
{
    [SerializeField] private float _baseValue;
    
    public float BaseValue => _baseValue;
}