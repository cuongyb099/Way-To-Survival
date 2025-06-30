
using System;
using DG.Tweening;
using UnityEngine;

public class WrenchBase : MeleeBase
{
    public override void MeleeLogic()
    {
        Physics.OverlapBoxNonAlloc(hitBox.bounds.center, hitBox.bounds.extents, hitColliders,Quaternion.identity);
        foreach (var x in hitColliders)
        {
            if(!playerController.CompareTag(x.gameObject.tag) || x.gameObject.layer != LayerMask.NameToLayer("FixableObject"))return;
            if(!x.TryGetComponent<StatsController>(out StatsController stats)) return;
            if (stats.TryGetAttribute(AttributeType.Hp, out var hpStats))
            {
                hpStats.Value += WeaponData.Damage.Value*hpStats.MaxValue;
            }
        }
    }
}
