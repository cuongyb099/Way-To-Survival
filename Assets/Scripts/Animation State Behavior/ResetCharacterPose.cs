using UnityEngine;

public class ResetCharacterPose : StateMachineBehaviour
{
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetFloat(GlobalAnimation.RandomPose, -1f);
    }
}
