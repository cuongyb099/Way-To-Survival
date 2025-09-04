using Tech.Pooling;
using UnityEngine;
using UnityEngine.VFX;

namespace DefaultNamespace.Constructor.Drum
{
    public class SpawnVisualEffectOnDeath : MonoBehaviour
    {
        [SerializeField] protected GameObject fxPrefab;
        
        protected virtual void Awake()
        {
            if (TryGetComponent(out IDamagable damagable))
            {
                damagable.OnDeath += () =>
                {
                    var fx = ObjectPool.Instance.SpawnObject(fxPrefab, transform.position, Quaternion.identity);
                    fx.GetComponent<VisualEffect>().Play();
                };
            }
        }
    }
}