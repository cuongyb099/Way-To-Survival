using System;
using UnityEngine;

namespace DefaultNamespace.Constructor.Drum
{
    public abstract class Damageable : MonoBehaviour, IDamagable
    {
        public bool IsDead { get; protected set; }
        public Action OnDamaged { get; set; }
        public Action OnDeath { get; set; }

        public abstract float Damage(DamageInfo info);

        public abstract void Death(GameObject dealer);

        public GameObject GetGameObject() => gameObject;
        
        protected virtual void OnEnable()
        {
            this.IsDead = false;
        }
    }
}