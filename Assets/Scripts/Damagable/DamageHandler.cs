using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Unity.Timeline;
using UnityEngine;

public static class DamageHandler
{
    public static void Damage(IDamagable target, DamageInfo damage)
    {
        if (target == null)
        {
            Debug.LogError(target.ToString() + " has no damagable component");
            return;
        }
        float finalDamage = target.Damage(damage);
        DamagePopUpGenerator.Instance.CreateDamagePopUp(target.GetGameObject().transform.position, (int)finalDamage, damage.IsCrit);
        if (damage.Dealer.CompareTag("Player"))
        {
            if (damage.DamageType == DamageType.Bullet)
            {
                PlayerEvent.OnBulletDamageDealt?.Invoke(finalDamage, target);
            }
            if (damage.DamageType == DamageType.Melee)
            {
                PlayerEvent.OnMeleeDamageDealt?.Invoke(finalDamage, target);
            }
            if (damage.DamageType == DamageType.FollowUp)
            {
                PlayerEvent.OnFollowUpDamageDealt?.Invoke(finalDamage, target);
            }
            PlayerEvent.OnDamageDealt?.Invoke(finalDamage, target);
        }
    }
}
