using System;
using KatInventory;
using UnityEngine;

[RequireComponent(typeof(IndicatorBuilding))]
public class Structure : ItemBase, IDamagable
{
    protected IndicatorBuilding indicator;
    protected Collider collision;
    
    [field: SerializeField] public PlaceHitBox PlacmentHitBox { get; private set; }

    protected virtual void Reset()
    {
        PlacmentHitBox = GetComponentInChildren<PlaceHitBox>();
    }

    protected virtual void Awake()
    {
        collision = GetComponent<Collider>();
        indicator = GetComponent<IndicatorBuilding>();
        
        Stats = GetComponent<StatsController>();
        Stats.ReInit();
        isDead = false;
    }

    public virtual void SetIndicator(Color color)
    {
        collision.enabled = false;
        indicator.SetIndicator(color);
    }

    public virtual void ReturnDefault()
    {
        collision.enabled = true;
        indicator.ReturnDefault();   
    }
    
    public StatsController Stats { get; private set; }
    public bool IsDead => isDead;
    protected bool isDead;

    public Action OnDamaged { get; set; }
    public Action OnDeath { get; set; }

    public virtual float Damage(DamageInfo info)
    {
        if(isDead) return 0;
        float finalDamage = Mathf.Clamp(info.Damage - Stats.GetStat(StatType.DEF).Value, 0, 999999);
        Stats.GetAttribute(AttributeType.Hp).Value -= finalDamage;
        OnDamaged?.Invoke();
        if (Stats.GetAttribute(AttributeType.Hp).Value <= 0)
        {
            Stats.GetAttribute(AttributeType.Hp).Value = 0;
            Death(info.Dealer);
        }

        return finalDamage;
    }

    public virtual void Death(GameObject dealer)
    {
        Debug.Log(gameObject.name + "has been slain!!");
        isDead = true;
        OnDeath?.Invoke();
    }

    public GameObject GetGameObject() => gameObject;

    [ContextMenu("Deal 10 DMG")]
    public void Damage10Dmg()
    {
        Damage(new DamageInfo(gameObject,10));
    }
}