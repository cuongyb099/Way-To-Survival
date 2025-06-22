using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using Tech.Logger;
using UnityEngine;

[TaskCategory("Utilities")]
[TaskDescription("Sets a trigger parameter to active or inactive. Returns Success.")]
public class AnimationSetTrigger : Action
{
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
    public SharedGameObject targetGameObject;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("The name of the parameter")]
    public SharedString paramaterName;
    private int hashParamaterName;
    private Animator animator;
    private GameObject prevGameObject;

    public override void OnAwake()
    {
        hashParamaterName = Animator.StringToHash(paramaterName.Value);
    }

    public override void OnStart()
    {
        var currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        if (currentGameObject != prevGameObject) {
            animator = currentGameObject.GetComponent<Animator>();
            prevGameObject = currentGameObject;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (animator == null) {
            LogCommon.LogWarning("Animator is null");
            return TaskStatus.Failure;
        }

        animator.SetTrigger(hashParamaterName);

        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        targetGameObject = null;
        paramaterName = "";
    }
}