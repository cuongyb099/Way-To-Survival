using BehaviorDesigner.Runtime.Tasks;

public class BaseEnemyBehavior : Action
{
    protected EnemyCtrl enemyCtrl;
    
    public override void OnAwake()
    {
        base.OnAwake();
        
        enemyCtrl = GetComponent<EnemyCtrl>();
    }
}