using UnityEngine;

[RequireComponent(typeof(StatsController))]
public class PlayerStats : MonoBehaviour
{
   /* private StatsController _statsController;

    private Attribute _hp;
    private Attribute _mana;

    private Stat _maxHp;
    private Stat _maxMana;
    
    private void Awake()
    {
        _statsController = GetComponent<StatsController>();
    }

    private void Start()
    {
        if (_statsController.TryGetAttribute(AttributeType.Hp, out _hp))
        {
            _hp.OnValueChange += HandleHearthChange;
            GameEvent.OnInitStatusBar?.Invoke(AttributeType.Hp, _hp.Value, _hp.MaxValue);
        }

        if (_statsController.TryGetAttribute(AttributeType.Mana, out _mana))
        {
            _mana.OnValueChange += HandleManaChange;
            GameEvent.OnInitStatusBar?.Invoke(AttributeType.Mana, _mana.Value, _mana.MaxValue);
        }

        if (_statsController.TryGetStat(StatType.MaxHp, out _maxHp))
        {
            _maxHp.OnValueChange += HandleMaxHpChange;
        }
        
        if (_statsController.TryGetStat(StatType.MaxMana, out _maxMana))
        {
            _maxMana.OnValueChange += HandleMaxManaChange;
        }
    }

    private void HandleMaxManaChange()
    {
        GameEvent.OnMaxManaChange?.Invoke(_mana.Value, _maxMana.Value);
    }

    private void HandleMaxHpChange()
    {
        GameEvent.OnMaxHeathChange?.Invoke(_hp.Value, _maxHp.Value);
    }

    private void OnDestroy()
    {
        if(_statsController == null) return;
        _hp.OnValueChange -= HandleHearthChange;
        _mana.OnValueChange -= HandleManaChange;
        _maxHp.OnValueChange -= HandleMaxHpChange;
        _maxMana.OnValueChange -= HandleMaxManaChange;
    }

    private void HandleHearthChange()
    {
        if(_hp == null) return;
        GameEvent.OnHeathChange?.Invoke(_hp.Value, _hp.MaxValue);
    }
    
    private void HandleManaChange()
    {
        if(_mana == null) return;
        GameEvent.OnManaChange?.Invoke(_mana.Value, _mana.MaxValue);
    }*/
}
