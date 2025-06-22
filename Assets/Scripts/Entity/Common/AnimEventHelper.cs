using UnityEngine;
using UnityEngine.Events;

public class AnimEventHelper : MonoBehaviour
{
    [HideInInspector] public bool IsCurrentAnimEnd;

    public UnityEvent CallbackAnimEnd, CallbackAnimEvent;
    
    public void OnAnimationEnd()
    {
        IsCurrentAnimEnd = true;
        CallbackAnimEnd?.Invoke();
    }

    public void AnimationTrigger()
    {
        CallbackAnimEvent?.Invoke();
    }
}
