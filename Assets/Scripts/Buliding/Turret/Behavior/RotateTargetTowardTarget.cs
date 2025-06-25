using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using DG.Tweening;
using UnityEngine;

[TaskCategory("AI")]
public class RotateTargetTowardTarget : Action
{
    public SharedTransform TargetRotate;
    public SharedTransform Target;
    public SharedFloat Duration = 0.5f;
    protected Tween rotationTween;
    
    public override void OnStart()
    {
        var lookDir = Target.Value.position - TargetRotate.Value.position;
        lookDir.y = 0;
        var rotation = Quaternion.LookRotation(lookDir.normalized, Vector3.up);
        rotationTween = TargetRotate.Value.DORotateQuaternion(rotation, Duration.Value);
    }

    public override TaskStatus OnUpdate()
    {
        return rotationTween.IsActive() ? TaskStatus.Running : TaskStatus.Success;
    }
}