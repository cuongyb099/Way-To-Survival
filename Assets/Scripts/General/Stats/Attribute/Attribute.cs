using System;
using UnityEngine;

[Serializable]
public class Attribute
{
	private float _value;
	private float _minValue;
	private Stat _maxValue;
	private StatsController _controller;

	public float MinValue => _minValue;
	public float MaxValue => _maxValue?.Value ?? 0f;


	public float Value
	{
		get
		{
			_value = Math.Clamp(_value, _minValue, MaxValue);

			return _value;
		}
		set
		{
			var newValue = Math.Clamp(value, _minValue, MaxValue);
			
			if (newValue != _value)
			{
				_value = newValue;
				OnValueChange?.Invoke();
			}
		}
	}

	public Action OnValueChange;

	public Attribute(float minValue, Stat maxValue, float startPercent, StatsController controller)
	{
		_minValue = minValue;
		_maxValue = maxValue;
		_controller = controller;

		_value = Mathf.Lerp(minValue, MaxValue, startPercent);
	}
	public void SetValueToMax()
	{
		Value = MaxValue;
	}
	
	public void ReInit(float startPercent)
	{
		_value = Mathf.Lerp(_minValue, _maxValue.Value, startPercent);
		OnValueChange?.Invoke();
	}
}