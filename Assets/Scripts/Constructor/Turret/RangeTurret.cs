using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Core.Animation_Event_System;
using DefaultNamespace.Buliding.Turret;
using DefaultNamespace.Projectile;
using KatInventory;
using Tech.Pooling;
using UnityEngine;
using ObjectPool = Tech.Pooling.ObjectPool;

public class RangeTurret : Structure
{
    protected BehaviorTree bTree;
    protected TurretData turretData;
    [SerializeField] protected AnimationEventReceiver gunAnimEventCtrl;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected List<Transform> bulletSpawnPoints;
    [SerializeField] protected Transform gunTransform;
    protected override void Awake()
    {
        base.Awake();
        bTree = GetComponent<BehaviorTree>();
        OnDeath += () =>
        {
            this.gameObject.SetActive(false);
        };
        
        gunAnimEventCtrl.Subscribe(TurretAnimEventID.Shot, (int gunIndex) =>
        {
            var bulletSpawnPoint = bulletSpawnPoints[gunIndex];
            var bulletClone = ObjectPool.Instance.SpawnObject(bulletPrefab, bulletSpawnPoint.position, default);
            bulletClone.transform.rotation = bulletSpawnPoint.transform.rotation;
           
            if (bulletClone.TryGetComponent(out IProjectile projectile))
            {
                projectile.Init(gunTransform.forward ,10);
            }

            if (bulletClone.TryGetComponent(out IDamageDealer damageDealer))
            {
                damageDealer.SetDamage(new DamageInfo()
                {
                    Damage = 5f,
                    Dealer = this.gameObject
                });
            }
        });
    }

    public override void SetIndicator(Color color)
    {
        base.SetIndicator(color);
        bTree.enabled = false;
    }

    public override void ReturnDefault()
    {
        base.ReturnDefault();
        bTree.enabled = true;
    }

    public override void SetData(ItemData itemData)
    {
        turretData = itemData as TurretData;
    }
    
    
}
