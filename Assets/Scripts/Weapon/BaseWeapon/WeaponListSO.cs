using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "WeaponListSO",menuName ="Item/Weapon/new WeaponListSO")]
public class WeaponListSO : ScriptableObject
{
    public List<WeaponBaseSO> Weapons;
}
