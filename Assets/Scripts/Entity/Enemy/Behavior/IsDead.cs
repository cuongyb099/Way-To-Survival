using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class IsDead : Conditional
{
    private EnemyCtrl _enemyCtrl;

    public override void OnAwake()
    {
        _enemyCtrl = GetComponent<EnemyCtrl>();
    }

    public override TaskStatus OnUpdate()
    {
        return _enemyCtrl.IsDead ? TaskStatus.Success : TaskStatus.Failure;
    }
}