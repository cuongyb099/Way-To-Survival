using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

namespace BTreeNode
{
    [TaskCategory("Utilities")]
    public class WaitUntil : Action
    {
        public SharedBool Target;

        public override TaskStatus OnUpdate()
        {
            if (!Target.Value) return TaskStatus.Running;
            Target.Value = false;
            return TaskStatus.Success;
        }
    }
}