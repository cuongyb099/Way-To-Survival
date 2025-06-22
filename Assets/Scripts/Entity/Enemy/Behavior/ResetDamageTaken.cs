using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("AI")]
public class ResetDamageTaken : BaseEnemyBehavior
{
    public override TaskStatus OnUpdate()
    {
        enemyCtrl.IsTakingDamage = false;
        return TaskStatus.Success;
    }
}