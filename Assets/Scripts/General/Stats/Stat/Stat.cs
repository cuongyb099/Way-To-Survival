using System;
using System.Collections.Generic;

[Serializable]
public class Stat
{
	private float _baseValue;
	public float BaseValue
	{
		get { return _baseValue; }
		set { _baseValue = value; _isDirty = true; }
	}

	private bool _isDirty = true;

	private float _value;
	public virtual float Value
	{
		get
		{
			if (_isDirty)
			{
				_value = CalculateFinalValue();
				_isDirty = false;
			}
			return _value;
		}
	}

	public Action OnValueChange;

	protected readonly List<StatModifier> statModifiers;
	public List<StatModifier> StatModifiers => statModifiers;

	public Stat()
	{
		statModifiers = new List<StatModifier>();
	}

	public Stat(float baseValue) : this()
	{
		_baseValue = baseValue;
	}
	
	public void ClearAllModifiers()
	{
		statModifiers.Clear();
		_isDirty = true;
		OnValueChange?.Invoke();
	}
	
	public virtual void AddModifier(StatModifier mod)
	{
		_isDirty = true;
		statModifiers.Add(mod);
		OnValueChange?.Invoke();
	}

	public virtual bool RemoveModifier(StatModifier mod)
	{
		if (statModifiers.Remove(mod))
		{
			_isDirty = true;
			OnValueChange?.Invoke();
			return true;
		}
		return false;
	}

	public virtual bool RemoveAllModifiersFromSource(object source)
	{
		int numRemovals = statModifiers.RemoveAll(mod => mod.Source == source);

		if (numRemovals > 0)
		{
			_isDirty = true;
			OnValueChange?.Invoke();
			return true;
		}
		return false;
	}

	protected virtual float CalculateFinalValue()
	{
        float _baseFlatSum = 0f;
        float _percentageSum = 0f;
        float _flatSum = 0f;

        foreach (StatModifier modifier in StatModifiers)
        {
            if (modifier.Type == StatModType.BaseFlat)
            {
                _baseFlatSum += modifier.Value;
            }
            else if (modifier.Type == StatModType.Percentage)
            {
                _percentageSum += modifier.Value;
            }
            else if (modifier.Type == StatModType.Flat)
            {
                _flatSum += modifier.Value;
            }
        }
        float _finalValue = (BaseValue + _baseFlatSum) * (1f + _percentageSum / 100f) + _flatSum;


        return (float)Math.Round(_finalValue, 4);
	}
}
