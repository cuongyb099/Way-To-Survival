using System.Collections;
using UnityEngine;

public class ShotgunBase : GunBase
{
    public float BulletsPerShot = 5f;
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