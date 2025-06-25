using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

public class TurretShoot : Action
{
    public SharedTransform GunTransform;
    public GameObject Bullet;
    public SharedFloat CoolDownTime;
    public float GunRecoilStrength = 1;
    public float RecoilTime = 0.1f;
    protected Vector3 originPos;
    protected float timer;
    
    public override void OnStart()
    {
        originPos = GunTransform.Value.position;
        GunTransform.Value.DOMove(GunTransform.Value.position - GunTransform.Value.forward * GunRecoilStrength, RecoilTime)
            .OnComplete(() =>
            {
                GunTransform.Value.DOMove(originPos, RecoilTime);
            });
        timer = 0;
    }

    public override TaskStatus OnUpdate()
    {
        if (timer < CoolDownTime.Value)
        {
            timer += Time.deltaTime;
            return TaskStatus.Running;
        }
        
        return TaskStatus.Success;
    }
}