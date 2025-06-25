using System.Collections.Generic;
using KatInventory;
using Newtonsoft.Json;
using UnityEngine;

[JsonConverter(typeof(ItemSoConverter))]
[CreateAssetMenu(fileName = "GunSO",menuName ="Item/Gun/new GunSO")]
public class GunBaseSO : WeaponBaseSO
{
	[field:Header("Gun Data")]
	[field: SerializeField] public GameObject BulletPrefab{ get; private set; }
	[field: SerializeField] public GameObject ShellPrefab{ get; private set; }
	[field: SerializeField] public GameObject MagPrefab{ get; private set; }
	[field: SerializeField] public float Aim{ get; private set; }
	[field: SerializeField] public float SpreadMax{ get; private set; }
	[field: SerializeField] public float Recoil{ get; private set; }
    [field: SerializeField] public float RecoilResetTime{ get; private set; }
	[field: SerializeField] public float MaxCapacity{ get; private set; }
	[field: SerializeField] public WeaponType GunReloadType{ get; private set; }
	
    [field:Header("Gun Sounds")]
    [field: SerializeField] public AudioClip TailSound{ get; private set; }
    [field: SerializeField] public AudioClip CockingSound{ get; private set; }
    [field: SerializeField] public AudioClip MagSoundIn{ get; private set; }
    [field: SerializeField] public AudioClip MagSoundOut{ get; private set; }
    
    public override ItemData CreateItemData(int quantity = 1)
    {
	    return new GunData(this, quantity);
    }
}
