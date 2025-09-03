using System.Collections;
using UnityEngine;

public class ShotgunBase : GunBase
{
    [field: SerializeField] public int BulletsPerShot { get; set; } = 5;

    void Start()
    {
        GunRecoil = 1f;
    }

    public override void BulletInstantiate()
    {
        for (int i = 0; i < BulletsPerShot; i++)
        {
            base.BulletInstantiate();
        }
    }
    public override void GunRecoilUpdate()
    {
    }
    public override void ResetRecoil()
    {
    }
}