using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Tech.Logger;
using UnityEngine;

[TaskCategory("AI")]
public class NormalAttack : BaseEnemyBehavior
{
    public SharedFloat Damage;
    public SharedString DetectionParam;
    public SharedString TargetTag;
    public SharedString AnimationName;
    private int _hashAnimation;
    private ColliderDetection _colliderDetection;
    private bool _canActive;

    public override void OnStart()
    {
        _canActive = true;
        enemyCtrl.Anim.SetTrigger(_hashAnimation);
        enemyCtrl.Anim.SetBool(GlobalAnimation.IsAttackAnimationEnd, false);
    }

    public override void OnAwake()
    {
        base.OnAwake();
        _colliderDetection = enemyCtrl.GetComponentInChildren<ColliderDetectionCtrl>()
            .GetDetection(DetectionParam.Value);
        if (_colliderDetection == null)
        {
            LogCommon.LogError(DetectionParam + "Not Found");
        }
        _hashAnimation = Animator.StringToHash(AnimationName.Value);
        var _animationEventHelper = enemyCtrl.GetComponentInChildren<AnimationEventHelper>();
        
        //If Not Dispose Event We Should Use Pause OnDisable To Pool
        _animationEventHelper.OnAnimationTrigger.AddListener(SetActive);
        _colliderDetection.CallbackTriggerEnter += DealDamage;
    }

    public override void OnPause(bool paused)
    {
        var _animationEventHelper = enemyCtrl.GetComponentInChildren<AnimationEventHelper>();
        _animationEventHelper.OnAnimationTrigger.RemoveListener(SetActive);
    }

    public override TaskStatus OnUpdate()
    {
        return enemyCtrl.Anim.GetBool(GlobalAnimation.IsAttackAnimationEnd) ? TaskStatus.Success : TaskStatus.Running;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        _colliderDetection.SetActiveDetect(false);
    }

    private void SetActive(string eventName)
    {
        if(!_canActive) return;
        _colliderDetection.SetActiveDetect(true);
    }
    
    private void DealDamage(Collider target)
    {
        if(!target.CompareTag(TargetTag.Value)) return;
        
        if (!target.TryGetComponent(out IDamagable damageable)) return;
        
        
        damageable.Damage(new DamageInfo()
        {
            Damage = Damage.Value,
        });
        _colliderDetection.SetActiveDetect(false);
    }
}