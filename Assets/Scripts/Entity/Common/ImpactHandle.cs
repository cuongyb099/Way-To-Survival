using Tech.Pooling;
using UnityEngine;

public class ImpactHandle : MonoBehaviour, IDamageDealer
{
    [SerializeField] protected GameObject ImpactPrefab;
    protected DamageInfo damage;
    public bool Done {get; private set;}
    [SerializeField] protected bool disableOnImpact;
    [SerializeField] protected bool stopOnImpact;
    protected virtual void OnEnable()
    {
        Done = false;
    }

    public void SetDamage(DamageInfo damageInfo)
    {
        damage = damageInfo;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if(IsValid(other.gameObject)) return;

        Check(other.ClosestPointOnBounds(transform.position));

        DealDamage(other.transform);
    }

    protected virtual void OnCollisionEnter(Collision other)
    {
        if(IsValid(other.gameObject)) return;

        Check(other.GetContact(0).point);

        DealDamage(other.transform);
    }

    protected virtual bool IsValid(GameObject target){
        return Done || (damage.Dealer && target.CompareTag(damage.Dealer.tag));
    }

    protected virtual void Check(Vector3 contactPos){
        Done = true;

        if(ImpactPrefab){
            //Avoid Zfighting Painting Texture
            ObjectPool.Instance.SpawnObject(ImpactPrefab, contactPos + new Vector3(0.05f, 0.05f, 0.05f), 
                Quaternion.identity, PoolType.GameObject);
        }

        if(disableOnImpact){
            gameObject.SetActive(false);
        }

        if(stopOnImpact && TryGetComponent(out IFlyable flyable)){
            flyable.Stop();
        }
    }

    protected virtual void DealDamage(Transform target){
        if(!target.TryGetComponent(out IDamagable damagable)) return;

        damagable.Damage(damage);
    }
}