using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BTreeNode.SharedType
{
    [TaskCategory("Animator")]
    public class AnimatorPlay : Action
    {
        public SharedString LastPlayAnim;
        public SharedString AnimName;
        public SharedInt Layer;
        protected Animator animator;
        
        public override void OnAwake()
        {
            base.OnAwake();
            animator = this.transform.GetComponentInChildren<Animator>();
        }

        public override TaskStatus OnUpdate()
        {
            if(LastPlayAnim.Value == AnimName.Value) return TaskStatus.Success;
            animator.Play(AnimName.Value, Layer.Value);
            LastPlayAnim.Value = AnimName.Value;
            return TaskStatus.Success;
        }
    }
}