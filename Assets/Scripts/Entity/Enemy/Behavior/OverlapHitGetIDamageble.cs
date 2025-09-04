using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BTreeNode.SharedType;

namespace Entity.Enemy.Behavior
{
    [TaskCategory("Job System")]
    public class OverlapHitGetIDamageble : Conditional
    {
        public SharedListCollider HitResults;
        public SharedTransform TargetResult;
        
        public override TaskStatus OnUpdate()
        {
            foreach (var collider in HitResults.Value)
            {
                if (!collider.TryGetComponent(out IDamagable damagable) || damagable.IsDead) continue;
                TargetResult.Value = collider.transform;
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }
    }
}