using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using BTreeNode.SharedType;

namespace Entity.Enemy.Behavior
{
    [TaskCategory("Job System")]
    public class OverlapHitContain : Conditional
    {
        public SharedHashsetTransform HashsetTransform;
        public SharedTransform Target; 
        
        public override TaskStatus OnUpdate()
        {
            if(HashsetTransform.Value.Contains(Target.Value)) return TaskStatus.Success;
            return TaskStatus.Failure;
        }
    }
}