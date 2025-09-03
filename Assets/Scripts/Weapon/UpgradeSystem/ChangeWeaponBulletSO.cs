
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeGunBulletSO", menuName = "Buff/WeaponUpgrade/new ChangeGunBulletSO")]
public class ChangeGunBulletSO : WeaponSkillUnlockSO
{
    [field:Header("Gun Upgrade Data")]
    [field:SerializeField] public GameObject UpgradedBullet { get; private set; }
    
    public override void ApplySkill(WeaponBase weapon)
    {
        if (weapon is not GunBase) return;
        
        ((GunBase)weapon).GunBulletPrefab = UpgradedBullet;
    }

    public override void RemoveSkill(WeaponBase weapon)
    {
        if (weapon is not GunBase) return;
        ((GunBase)weapon).GunBulletPrefab = null;
    }
}
