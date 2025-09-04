using Tech.Pooling;
using UnityEngine;

namespace DefaultNamespace.Constructor.Drum
{
    public class SpawnParticleOnDeath : MonoBehaviour
    {
        [SerializeField] protected GameObject fxPrefab;
        
        protected virtual void Awake()
        {
            if (TryGetComponent(out IDamagable damagable))
            {
                damagable.OnDeath += () =>
                {
                    ObjectPool.Instance.SpawnObject(fxPrefab, transform.position + Vector3.up * 0.05f, Quaternion.identity);
                };
            }
        }
    }
}