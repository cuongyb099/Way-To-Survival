using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
[CanEditMultipleObjects]
#endif
public class StatsController : MonoBehaviour
{
	[SerializeField] private StatsHolderSO _statsHolder;
	protected Dictionary<StatType, Stat> _stats;
	protected Dictionary<AttributeType, Attribute> _attributes;

	public List<BaseStatusEffect> StatusEffectsList => _statusEffects;
	protected List<BaseStatusEffect> _statusEffects = new List<BaseStatusEffect>();

	public Dictionary<StatType, Stat> Stats
	{
		get
		{
			InitStats();
			return _stats;
		}
	}

	public Dictionary<AttributeType, Attribute> Attributes
	{
		get
		{
			InitAttribute();
			return _attributes;
		}
	}

	public List<BaseStatusEffect> StatusEffects => _statusEffects;

	public Action OnChange;

	protected virtual void Awake()
	{
		InitAttribute();
	}
	
	public void ReInit()
	{
		foreach (Stat stat in _stats.Values)
		{
			stat.ClearAllModifiers();
		}
	
		foreach (AttributeType key in _statsHolder.AttributeItems.Keys)
		{
			AttributeItem attributeItem = _statsHolder.GetAttribute(key);
		
			_attributes[key].ReInit(attributeItem.StartPercent);
		}
	}
	
	protected virtual void InitAttribute()
	{
		if (!_statsHolder || _attributes != null)
		{
			return;
		}
		
		InitStats();
		
		_attributes = new Dictionary<AttributeType, Attribute>();
		foreach (AttributeType key in _statsHolder.AttributeItems.Keys)
		{
			AttributeItem attributeItem = _statsHolder.GetAttribute(key);

			Attribute attribute = new(attributeItem.MinValue, _stats[attributeItem.MaxValue], attributeItem.StartPercent, this);
			attribute.OnValueChange += () => OnChange?.Invoke();
			_attributes.Add(key, attribute);
		}
	}

	protected virtual void InitStats()
	{
		if (!_statsHolder || _stats != null)
		{
			return;
		}

		_stats = new Dictionary<StatType, Stat>();
		foreach (StatType key in _statsHolder.StatItems.Keys)
		{
			Stat stat = new(_statsHolder.StatItems[key].BaseValue);
			stat.OnValueChange += () => OnChange?.Invoke();
			_stats.Add(key, stat);
		}
	}

	public virtual void AddModifier(StatType type, StatModifier modifier)
	{
		if (!_stats.TryGetValue(type, out Stat value)) return;
	
		value.AddModifier(modifier);
	}

	public virtual void RemoveModifier(StatType type, StatModifier modifier)
	{
		_stats[type].RemoveModifier(modifier);
	}

	public void SubtractAttributeValue(AttributeType type, float value)
	{
		if (TryGetAttribute(type, out Attribute attribute))
		{
			attribute.Value -= value;
		}
	}

	public bool TryGetAttribute(AttributeType type, out Attribute attribute)
	{
		if (_attributes.TryGetValue(type, out var value))
		{
			attribute = value;
			return true;
		}

		attribute = null;
		return false;
	}

	public bool TryGetStat(StatType type, out Stat stat)
	{
		if (_stats.TryGetValue(type, out var value))
		{
			stat = value;
			return true;
		}

		stat = null;
		return false;
	}

	private void UpdateStatusEffect()
	{
		for (int i = _statusEffects.Count - 1; i >= 0; --i)
		{
			var effect = _statusEffects[i];
			if (effect.ForceStop)
			{
				effect.Stop();
				_statusEffects.RemoveAt(i);
				OnChange?.Invoke();
				continue;
			}
			
			if (!effect.MonoUpdate()) continue;

			effect.Stop();
			_statusEffects.RemoveAt(i);
			OnChange?.Invoke();
		}
	}

	public void ApplyEffect(BaseStatusEffect effect)
	{
		if (!effect.Data.Stackable)
		{
			if (_statusEffects.Contains(effect)) return;
			effect.Begin();
			_statusEffects.Add(effect);
			OnChange?.Invoke();
			return;
		}
		
		if (_statusEffects.Contains(effect))
		{
			BaseStatusEffect sf = _statusEffects.Find(x => x.Equals(effect));
			if (sf.CurrentStack >= sf.Data.MaxStack) return;
			sf.CurrentStack++;
			OnChange?.Invoke();
			return;
		}
		
		effect.Begin();
		effect.CurrentStack++;
		_statusEffects.Add(effect);
		OnChange?.Invoke();
	}

	public void RemoveEffect(BaseStatusEffect effect)
	{
		if (!effect.Data.Stackable)
		{
			if (_statusEffects.Contains(effect))
			{
				BaseStatusEffect sf = _statusEffects.Find(x => x.Equals(effect));
				sf.Stop();
				_statusEffects.Remove(sf);
				OnChange?.Invoke();
			}
			return;
		}
		
		if (_statusEffects.Contains(effect))
		{
			BaseStatusEffect sf = _statusEffects.Find(x => x.Equals(effect));
			sf.CurrentStack--;
			if (sf.CurrentStack == 0)
			{
				sf.Stop();
				_statusEffects.Remove(effect);
			}
			OnChange?.Invoke();
		}
	}
	public void RemoveEffect(BaseBuffSO buff)
	{
		if (!buff.Stackable)
		{
			if (_statusEffects.Any(x=>x.Data.ID == buff.ID))
			{
				BaseStatusEffect sf = _statusEffects.Find(x=>x.Data.ID == buff.ID);
				sf.Stop();
				_statusEffects.Remove(sf);
				OnChange?.Invoke();
			}
			return;
		}
		
		if (_statusEffects.Any(x=>x.Data.ID == buff.ID))
		{
			BaseStatusEffect sf = _statusEffects.Find(x=>x.Data.ID == buff.ID);
			sf.CurrentStack--;
			if (sf.CurrentStack == 0)
			{
				sf.Stop();
				_statusEffects.Remove(sf);
			}
			OnChange?.Invoke();
		}
	}

	public Attribute GetAttribute(AttributeType type)
	{
		return _attributes.GetValueOrDefault(type);
	}

	public Stat GetStat(StatType type)
	{
		return _stats.GetValueOrDefault(type);
	}

	private void Update()
	{
		UpdateStatusEffect();
	}

	public bool HasEffect(BaseBuffSO target){
		foreach (var effect in _statusEffects)
		{
			if (effect.Data.ID == target.ID) return true;
		}
		return false;
	}
}

