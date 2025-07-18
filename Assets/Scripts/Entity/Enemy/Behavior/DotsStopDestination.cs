using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("NavMesh")]
public class DotsStopDestination : BaseEnemyBehavior
{
    public override TaskStatus OnUpdate()
    {
        enemyCtrl.StopDestination();
        return TaskStatus.Success;
    }
}