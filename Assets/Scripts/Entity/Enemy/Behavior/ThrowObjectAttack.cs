using BehaviorDesigner.Runtime;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ObjectPool = Tech.Pooling.ObjectPool;

public class ThrowObjectAttack : BaseEnemyBehavior
{
    private static readonly int Property = Animator.StringToHash("Is Attack End");
    private Transform _bulletSpawnPoint;
    public const string BulletSpawnPointName = "Bullet Spawn Point";
    public const string Scream = "Scream";
    public SharedTransform Target;
    public SharedGameObject Prefab;
    public SharedFloat ThrowAngle = 25f;
    public SharedFloat Cooldown = 3f;
    public SharedFloat Atk = 2f;
    public SharedFloat Force = 20f;
    public SharedBool IsCooldown;
    private AnimationEventHelper animationHelper;
    private Transform lastTarget;
    
    public override void OnAwake()
    {
        base.OnAwake();
        animationHelper = GetComponent<AnimationEventHelper>();
        animationHelper.OnAnimationTrigger.AddListener(HandleTrigger);
        _bulletSpawnPoint = enemyCtrl.Anim.GetBoneTransform(HumanBodyBones.Head).Find(BulletSpawnPointName);
        
    }

    public override void OnStart()
    {
        base.OnStart();
        lastTarget = Target.Value;
    }

    public override void OnReset()
    {
        animationHelper.OnAnimationTrigger.RemoveListener(HandleTrigger);
    }

    private void HandleTrigger(string name)
    {
        if(name != Scream) return;
        
        ThrowObject();        
    }

    private void ThrowObject()
    {
        if(IsCooldown.Value) return;
        enemyCtrl.Anim.SetBool(Property, false);
        var clone = ObjectPool.Instance.SpawnObject(Prefab.Value, _bulletSpawnPoint.position, 
            Quaternion.identity, Tech.Pooling.PoolType.ParticleSystem);


        if (clone.TryGetComponent(out IFlyable flyable))
        {
            flyable.ApplyFly(lastTarget.position - transform.position, Force.Value);
        }

        if (clone.TryGetComponent(out IDamageDealer damageDealer))
        {
            damageDealer.SetDamage(new DamageInfo(gameObject, Atk.Value * 0.65f));
        }
        
        
        IsCooldown.Value = true;

        _= StartCooldown();
    }

    private async UniTaskVoid StartCooldown(){
        await UniTask.WaitForSeconds(Cooldown.Value);

        IsCooldown = false;
    }
    
    
}