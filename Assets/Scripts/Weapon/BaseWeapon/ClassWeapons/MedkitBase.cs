
using DG.Tweening;
using UnityEngine;

public class MedkitBase : MeleeBase
{
    private Attribute hp;

    public override void Initialize()
    {
        base.Initialize();
        playerController.Stats.TryGetAttribute(AttributeType.Hp, out hp);
    }
    public override void MeleeLogic()
    {
        if(hp != null)
            hp.Value += (hp.MaxValue*WeaponData.Damage.Value);
        
        Physics.OverlapBoxNonAlloc(hitBox.bounds.center, hitBox.bounds.extents, hitColliders,Quaternion.identity);
        foreach (var x in hitColliders)
        {
            if(x.gameObject == playerController.gameObject || !playerController.CompareTag(x.gameObject.tag) || x.gameObject.layer == LayerMask.NameToLayer("FixableObject"))return;
            if(!x.TryGetComponent<StatsController>(out StatsController stats)) return;
            if (stats.TryGetAttribute(AttributeType.Hp, out var hpStats))
            {
                hpStats.Value += WeaponData.Damage.Value * hpStats.MaxValue;
            }
        }

    }
}
