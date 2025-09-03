using UnityEngine;

namespace DefaultNamespace.Constructor.Bared_Wire
{
    public class BarbedWire : BasicController
    {
        public override float Damage(DamageInfo info)
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

            if (info.Dealer && info.Dealer.TryGetComponent(out IDamagable damagable))
            {
                damagable.Damage(new DamageInfo()
                {
                    Damage = finalDamage * 0.5f,
                    Dealer = this.gameObject,
                });
            }
            
            return finalDamage;
        }
    }
}