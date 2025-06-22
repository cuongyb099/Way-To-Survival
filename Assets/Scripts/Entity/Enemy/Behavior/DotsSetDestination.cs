using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("AI")]
public class DotsSetDestination : BaseEnemyBehavior
{
    public SharedTransform Target;
    public override TaskStatus OnUpdate()
    {
        if (Target.Value == null)
        {
            return TaskStatus.Failure;
        }
        
        enemyCtrl.SetDestination(Target.Value.position);
        return TaskStatus.Success;
    }
}