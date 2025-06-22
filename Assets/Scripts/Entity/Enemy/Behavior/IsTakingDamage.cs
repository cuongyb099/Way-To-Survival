using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("AI")]
public class IsTakingDamage : Conditional
{
    private EnemyCtrl _enemyCtrl;

    public override void OnAwake()
    {
        _enemyCtrl = GetComponent<EnemyCtrl>();
    }

    public override TaskStatus OnUpdate()
    {
        return _enemyCtrl.IsTakingDamage ? TaskStatus.Success : TaskStatus.Failure;
    }
}