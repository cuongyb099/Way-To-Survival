using UnityEngine;

public class IsDeadAnimationEnd : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.SetBool(GlobalAnimation.IsDeadAnimationEnd, false);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.SetBool(GlobalAnimation.IsDeadAnimationEnd, true);
    }
}