using UnityEngine;
using UnityEngine.Events;

public class AnimationEventHelper : MonoBehaviour
{
    public UnityEvent<string> OnAnimationEnd, OnAnimationTrigger;

    public void AnimationEnd(string animationName)
    {
        OnAnimationEnd.Invoke(animationName);
    }

    public void AnimationTrigger(string animationName)
    {
        OnAnimationTrigger.Invoke(animationName);
    }
}
