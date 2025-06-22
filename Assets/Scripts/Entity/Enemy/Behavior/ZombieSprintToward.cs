using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class ZombieSprintToward : BaseEnemyBehavior
{
    public SharedTransform PlayerTransform;
    private StatModifier statModifier;
    private const string PlayerTag = "Player";
    private const string EnemyTag = "Enemy";
    private const string Param = "Hitbox Sprint Knockback";
    private ColliderDetection _hitbox;
    private Vector3 direction;
    [SerializeField] public float SpeedAdd = 5;
    public SharedFloat ATK;
    public static int Sprint = Animator.StringToHash("Sprint");
    private bool _isEnd;
    public SharedFloat Cooldown;
    public SharedBool Interrupted;
    public SharedBool IsSprint;
    public SharedLayerMask LayerMask;
    private RaycastHit[] _hits;
    private Rigidbody _rb;
    public override void OnAwake()
    {
        base.OnAwake();
        _hitbox = enemyCtrl.GetComponentInChildren<ColliderDetectionCtrl>().GetDetection(Param);
        _hitbox.CallbackTriggerEnter += KnockbackPlayer;
        _hits = new RaycastHit[10];
        _rb = GetComponent<Rigidbody>();
    }
    public override void OnStart()
    {
        base.OnStart();
        if(IsSprint.Value) return;
        direction = Vector3.ProjectOnPlane((PlayerTransform.Value.position - transform.position), Vector3.up).normalized;
        var targetPosition = PlayerTransform.Value.position + direction * 5f;
        var distance = Vector3.Distance(transform.position, targetPosition);
        
        int count = Physics.RaycastNonAlloc(transform.position + Vector3.up, direction, _hits, distance, LayerMask.Value);
        
        if (count > 0)
        {
            for (int i = 0; i < count; i++)
            {
                var hit = _hits[i];
                
                if (hit.transform.CompareTag(PlayerTag) || hit.transform.CompareTag(EnemyTag)) continue;
                targetPosition = hit.point - direction * 1.5f;
                //targetPostion = PlayerTransform.Value.position;
                break;
            }
        }
        
        enemyCtrl.SetDestination(targetPosition);
        statModifier = new StatModifier(SpeedAdd, StatModType.Flat, transform);
        enemyCtrl.Stats.AddModifier(StatType.Speed, statModifier);
        enemyCtrl.Anim.SetBool(Sprint, true);
        _hitbox.SetActiveDetect(true);
        IsSprint.Value = true;
        _isEnd = false;
    }

    private void KnockbackPlayer(Collider obj)
    {
        if (!obj.CompareTag(PlayerTag)) return;

        if (obj.TryGetComponent(out IKnockbackable knockbackable))
        {
            knockbackable.ApplyKnockback(transform.forward, 700f);
        }

        if (obj.TryGetComponent(out IDamagable damagable))
        {
            damagable.Damage(new DamageInfo(gameObject, ATK.Value * 1.5f, dmgType: DamageType.Melee));
        }
        
        SetUpEnd();
    }

    private void SetUpEnd()
    {
        if(_isEnd) return;
        enemyCtrl.StopDestination();
        enemyCtrl.Stats.RemoveModifier(StatType.Speed, statModifier);
        _hitbox.SetActiveDetect(false);
        enemyCtrl.Anim.SetBool(Sprint, false);
        _isEnd = true;
        IsSprint.Value = false;
    }

    public override TaskStatus OnUpdate()
    {
        if (_isEnd || enemyCtrl.ReachEndOfPath())
        {
            SetUpEnd();
            Interrupted.Value = true;
            _ = StartCooldown();
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }

    private async UniTaskVoid StartCooldown()
    {
        await UniTask.Delay((int)(Cooldown.Value * 1000f));
        
        Interrupted.Value = false;
    }
}