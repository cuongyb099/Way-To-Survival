using System;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BTreeNode.SharedType;

namespace Entity.Enemy.Behavior
{
    [TaskCategory("Job System")]
    public class OverlapHitGetIDamageble : Conditional
    {
        public SharedHashsetTransform HashsetTransform;
        public SharedTransform TargetResult;
        
        public override TaskStatus OnUpdate()
        {
            foreach (var value in HashsetTransform.Value)
            {
                if (!value.TryGetComponent(out IDamagable damagable) || damagable.IsDead) continue;
                TargetResult.Value = value;
                return TaskStatus.Success;
            }
            
            return TaskStatus.Failure;
        }
    }
}