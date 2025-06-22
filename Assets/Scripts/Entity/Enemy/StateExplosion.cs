using UnityEngine;

public class StateExplosion : StateMachineBehaviour
{
    public GameObject Prefab;

    [Range(0f, 1f)] public float NormalizeTime;
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.normalizedTime >= NormalizeTime)
        {
            
            animator.gameObject.SetActive(false); 
        }
    }
}
