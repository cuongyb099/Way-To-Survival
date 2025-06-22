using System;
using UnityEngine;

public class BasicController : MonoBehaviour, IDamagable
{
    public StatsController Stats { get; private set; }
    public bool IsDead => isDead;
    protected bool isDead;

    public Action OnDamaged { get; set; }
    public Action OnDeath { get; set; }

    protected virtual void Awake()
    {
        Stats = GetComponent<StatsController>();
        Stats.ReInit();
        isDead = false;
    }
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
