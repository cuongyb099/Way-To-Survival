using System;
using KatLib.Pooling;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DefaultNamespace.Constructor.Drum
{
    public class OverlapDamageSender : MonoBehaviour
    {
        public float Damage;
        public float Radius = 3f;
        public LayerMask Layer;
        protected static Collider[] hits;
        public float ForceKnockback = 10f;
        public float MaxAngleKnockback = 45f;
        
        protected virtual void Awake()
        {
            hits ??= new Collider[10];
        }

        public void DealDamage()
        {
            var count = Physics.OverlapSphereNonAlloc(this.transform.position, Radius, hits, Layer);
            
            if(count == 0) return;

            for (var i = 0; i < count; i++)
            {
                var hit = hits[i];
                if (hit.TryGetComponent(out IDamagable damagable))
                {
                    damagable.Damage(new DamageInfo()
                    {
                        Damage = Damage,
                        Dealer = this.gameObject
                    });

                    if (!damagable.IsDead || !hit.TryGetComponent(out IKnockbackable knockback)) continue;
                    
                    Vector3 direction = hit.transform.position - this.transform.position;
                    direction.y = 0;
                    direction.Normalize();
                    var angle = Random.Range(0, MaxAngleKnockback);
                    var target = Quaternion.Euler(0, 0, angle) * direction;
                    knockback.ApplyKnockback(target, ForceKnockback);
                }
            }
        }

        protected virtual void OnDrawGizmos()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(this.transform.position, Radius);
        }
    }
}