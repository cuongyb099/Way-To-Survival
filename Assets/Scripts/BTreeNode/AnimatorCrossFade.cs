using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

namespace BTreeNode
{
    [TaskCategory("Animator")]
    public class AnimatorCrossFade : Action
    {
        public SharedString LastPlayAnim;
        public SharedString AnimName;
        public SharedFloat Transition;
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
            animator.CrossFade(AnimName.Value, Transition.Value, Layer.Value);
            LastPlayAnim.Value = AnimName.Value;
            return TaskStatus.Success;
        }
    }
}