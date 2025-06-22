using UnityEngine;

public class IsAttackAnimationEnd : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool(GlobalAnimation.IsAttackAnimationEnd, true);
    }
}
