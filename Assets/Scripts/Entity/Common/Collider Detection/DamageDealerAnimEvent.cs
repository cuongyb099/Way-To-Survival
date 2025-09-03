using System.Collections.Generic;
using Core.Animation_Event_System;
using KatLib.Observer;
using UnityEngine;
using UnityEngine.Pool;

namespace Entity.Common.Collider_Detection
{
    public class DamageDealerAnimEvent : ColliderDetection, IDamageDealer
    {
        protected DamageInfo damage;
        protected HashSet<Collider> damagedCollider;
        [SerializeField] protected AnimationEventReceiver receiver;
        public GenericEnum PerformAttackEvt;
        public GenericEnum DoneEvt;
        
        protected override void Awake()
        {
            base.Awake();
            if (!receiver)
            {
                receiver = GetComponentInParent<AnimationEventReceiver>();
            }
            
            receiver.Subscribe(PerformAttackEvt, () =>
            {
                this.SetActiveDetect(true);
            });
            
            receiver.Subscribe(DoneEvt, () =>
            {
                this.SetActiveDetect(false);
            });
        }

        public void SetDamage(DamageInfo damageInfo)
        {
            damage = damageInfo;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);

            damagedCollider ??= GenericPool<HashSet<Collider>>.Get();
            
            if(damagedCollider.Contains(other)) return;
            
            if (other.TryGetComponent(out IDamagable damagable))
            {
                damagable.Damage(damage);
            }
            
            damagedCollider.Add(other);
        }

        public override void SetActiveDetect(bool active)
        {
            base.SetActiveDetect(active);

            if (damagedCollider == null) return;
            
            damagedCollider.Clear();
            GenericPool<HashSet<Collider>>.Release(damagedCollider);
            damagedCollider = null;
        }
    }
}