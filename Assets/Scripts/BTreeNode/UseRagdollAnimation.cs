using BehaviorDesigner.Runtime.Tasks;

public class UseRagdollAnimation : BaseEnemyBehavior
{
    public bool Enable = true;

    public override TaskStatus OnUpdate()
    {
        if (Enable)
        {
            enemyCtrl.RagdollAnimation.EnableRagdoll();
            return TaskStatus.Success;
        }
        
        enemyCtrl.RagdollAnimation.DisableRagdoll();
        return TaskStatus.Success;
    }
}