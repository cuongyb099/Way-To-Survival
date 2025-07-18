using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

[TaskCategory("NavMesh")]
public class SetActiveAgent : BaseEnemyBehavior
{
    public SharedBool Active;

    public override TaskStatus OnUpdate()
    {
        enemyCtrl.Authoring.enabled = Active.Value;
        enemyCtrl.AvoidAuthoring.enabled = Active.Value;
        enemyCtrl.NavMeshAuthoring.enabled = Active.Value;
        return TaskStatus.Success;
    }
}