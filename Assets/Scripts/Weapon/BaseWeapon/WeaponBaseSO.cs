
using System.Collections.Generic;
using KatInventory;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeaponSO",menuName ="Item/Weapon/new WeaponSO")]
public class WeaponBaseSO : ItemGOBaseSO
{
    [field:Header("Weapon Data")]
    [field: SerializeField] public WeaponType WeaponType{ get; private set; }
    [field: SerializeField] public float ShootingSpeed{ get; private set; }
    [field: SerializeField] public float Damage{ get; private set; }
    [field: SerializeField] public float Weight{ get; private set; }
    [field: SerializeField] public bool ReleaseToShoot{ get; private set; }
    [field: SerializeField] public BuffUnlockByLevel BuffUnlockByLevel{ get; private set; }
    [field:Header("Attack Sounds")]
    [field: SerializeField] public List<AudioClip> AttackSounds{ get; private set; }
    public override ItemType GetItemType() => ItemType.Weapon;
    
    public override ItemData CreateItemData(int quantity = 1)
    {
        return new WeaponData(this,quantity);
    }
}
