
using DG.Tweening;
using UnityEngine;

public class MedkitBase : MeleeBase
{
    private Attribute hp;
    public override void Shoot()
    {
        
        if (!ShootAble ||
            !repeatAble ) return;
        repeatAble = false;
        PlayerEvent.OnAttack?.Invoke();
        playerController.Animator.SetFloat("MeleeCombo", ++comboCount%2);
        DOVirtual.DelayedCall(WeaponData.ShootingSpeed.Value, () => { repeatAble = true;});
        WeaponSoundPlay();
        if(hp != null)
            hp.Value += (hp.MaxValue*WeaponData.Damage.Value);
    }

    public override void Initialize()
    {
        base.Initialize();
        playerController.Stats.TryGetAttribute(AttributeType.Hp, out hp);
    }
}
