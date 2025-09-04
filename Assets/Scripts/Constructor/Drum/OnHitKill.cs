using System;
using UnityEngine;

namespace DefaultNamespace.Constructor.Drum
{
    public class OnHitKill : Damageable
    {
        public override float Damage(DamageInfo info)
        { 
            OnDamaged?.Invoke();
            Death(info.Dealer);
            return 1;
        }

        public override void Death(GameObject dealer)
        {
            OnDeath?.Invoke();
            this.IsDead = true;
            this.gameObject.SetActive(false);
        }
    }
}