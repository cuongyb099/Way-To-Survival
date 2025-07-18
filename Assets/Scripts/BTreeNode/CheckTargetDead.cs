using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BTreeNode
{
    [TaskCategory("AI")]
    public class CheckTargetDead : Action
    {
        public SharedTransform Target;
        public SharedTransform DefaultTarget;
        protected Transform lastTarget;
        protected IDamagable damagable;
        
        public override TaskStatus OnUpdate()
        {
            if (!lastTarget || lastTarget != Target.Value)
            {
                lastTarget = Target.Value;
                damagable = Target.Value.GetComponent<IDamagable>();
            }

            
            if (!damagable.IsDead) return TaskStatus.Success;

            if (Target.Value == DefaultTarget.Value)
            {
                this.Owner.DisableBehavior();    
                return TaskStatus.Success;
            }
            
            Target.Value = DefaultTarget.Value;

            return TaskStatus.Success;
        }
    }
}