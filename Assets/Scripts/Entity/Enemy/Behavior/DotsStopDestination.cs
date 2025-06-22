using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("AI")]
public class DotsStopDestination : BaseEnemyBehavior
{
    public override TaskStatus OnUpdate()
    {
        enemyCtrl.StopDestination();
        return TaskStatus.Success;
    }
}